using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace CSGL
{
	public class Game : GameWindow
	{
		private VertexBuffer vertexBuffer;
		private IndexBuffer indexBuffer;
		private VertexArray vertexArray;
		private ShaderProgram shaderProgram;

		private int vertexCount;
		private int indexCount;

		private Stopwatch timer;

		public Game(int width, int height, string title) : 
			base(GameWindowSettings.Default, 
				new NativeWindowSettings()
				{
					Title = title,
					ClientSize = new Vector2i(width, height),
					WindowBorder = WindowBorder.Fixed,
					StartVisible = false,
					StartFocused = true,
					API = ContextAPI.OpenGL,
					Profile = ContextProfile.Core,
					APIVersion = new Version(4, 1)
				})
		{

			this.CenterWindow();
		}

		float[] vertices = new float[100];
		uint[] indices = new uint[100];

		protected override void OnLoad()
		{
			timer = new Stopwatch();
			timer.Start();

			GL.ClearColor(0.3f, 0.4f, 0.5f, 1.0f);

			Random random = new Random();

			int windowWidth = this.ClientSize.X;
			int windowHeight = this.ClientSize.Y;

			int boxCount = 100;

			VertexPositionColour[] vertices = new VertexPositionColour[boxCount * 4];
			this.vertexCount = 0;

			for (int i = 0; i < boxCount; i++)
			{
				int w = random.Next(32, 128);
				int h = random.Next(32, 128);
				float x = random.Next(0, windowWidth - w);
				float y = random.Next(0, windowHeight - h);

				float r = (float)random.NextDouble();
				float g = (float)random.NextDouble();
				float b = (float)random.NextDouble();


				vertices[this.vertexCount++] = new VertexPositionColour(new Vector3(x, y + h, 0f), new Color4(r, g, b, 1f));
				vertices[this.vertexCount++] = new VertexPositionColour(new Vector3(x + w, y + h, 0f), new Color4(r, g, b, 1f));
				vertices[this.vertexCount++] = new VertexPositionColour(new Vector3(x + w, y, 0f), new Color4(r, g, b, 1f));
				vertices[this.vertexCount++] = new VertexPositionColour(new Vector3(x, y, 0f), new Color4(r, g, b, 1f));
			}

			uint[] indices = new uint[boxCount * 6];
			this.indexCount = 0;
			this.vertexCount = 0;

			for (int i = 0; i < boxCount; i++)
			{
				indices[this.indexCount++] = 0 + (uint)this.vertexCount;
				indices[this.indexCount++] = 1 + (uint)this.vertexCount;
				indices[this.indexCount++] = 2 + (uint)this.vertexCount;
				indices[this.indexCount++] = 0 + (uint)this.vertexCount;
				indices[this.indexCount++] = 2 + (uint)this.vertexCount;
				indices[this.indexCount++] = 3 + (uint)this.vertexCount;

				this.vertexCount += 4;
			}

			this.vertexBuffer = new VertexBuffer(VertexPositionColour.VertexInfo, vertices.Length, true);
			this.vertexBuffer.SetData(vertices, vertices.Length);

			this.indexBuffer = new IndexBuffer(indices.Length, true);
			this.indexBuffer.SetData(indices, indices.Length);

			this.vertexArray = new VertexArray(this.vertexBuffer);

			string vertexShaderCode =
				@"	#version 330

					uniform vec2 ViewportSize;
					uniform float ColourFactor;

					layout (location=0) in vec3 aPosition;
					layout (location=1) in vec4 aColour;

					out vec4 vColour;

					void main()
					{
						float nx = aPosition.x / ViewportSize.x * 2 - 1;
						float ny = aPosition.y / ViewportSize.y * 2 - 1;
						gl_Position = vec4(nx, ny, 0, 1.0f);

						vColour = aColour * ColourFactor;
					}";

			string fragmentShaderCode =
				@"	#version 330
					out vec4 pixelColor;

					uniform vec4 timeColour;

					in vec4 vColour;

					void main()
					{
						pixelColor = vec4(vColour); //vec4(timeColour); //vec4(0.8f, 0.8f, 0.1f, 1.0f);
					}";

			shaderProgram = new ShaderProgram(vertexShaderCode, fragmentShaderCode);

			int[] viewport = new int[4];

			GL.GetInteger(GetPName.Viewport, viewport);

			this.shaderProgram.SetUniform("ViewportSize", (float)viewport[2], (float)viewport[3]);
			this.shaderProgram.SetUniform("ColourFactor", 1);

			this.IsVisible = true;

			base.OnLoad();
		}


		// Is executed every frame
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			double timeValue = timer.Elapsed.TotalSeconds;
			
			float scalar = MathF.Sin((float)Time.time) / 2.0f + 0.5f;

			this.shaderProgram.SetUniform("ColourFactor", scalar);

			//Console.WriteLine("Time: " + Time.time + "\ndelta: " + Time.deltaTime);

			if (KeyboardState.IsKeyDown(Keys.Escape))
			{
				Close();
			}

			if (KeyboardState.IsKeyDown(Keys.D))
			{
				Console.WriteLine("Time: " + Time.time + "\ndelta: " + Time.deltaTime);
			}

			base.OnUpdateFrame(e);
		}

		protected override void OnRenderFrame(FrameEventArgs args)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			
			GL.UseProgram(this.shaderProgram.ShaderProgramHandle);
			GL.BindVertexArray(vertexArray.VertexArrayHandle);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer.IndexBufferHandle);

			GL.DrawElements(PrimitiveType.Triangles, this.indexCount, DrawElementsType.UnsignedInt, 0);

			this.Context.SwapBuffers();
			base.OnRenderFrame(args);
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);
			GL.Viewport(0, 0, e.Width, e.Height);
		}

		protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
		{
			GL.Viewport(0, 0, e.Width, e.Height);

			base.OnFramebufferResize(e);
		}

		protected override void OnUnload()
		{
			//GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			//GL.DeleteBuffer(this.elementBufferObject);

			this.vertexArray?.Dispose();
			this.vertexBuffer?.Dispose();
			this.indexBuffer?.Dispose();

			base.OnUnload();
		}
	}
}
