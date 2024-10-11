using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using System.Numerics;

namespace CSGL
{
	public class Game4 : GameWindow
	{
		private int vertexBufferObject;
		private int vertexArrayObject;
		private int shaderProgramObject;
		private int elementBufferObject;

		private Stopwatch timer;

		public Game4(int width, int height, string title) : 
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

		float[] vertices;
		uint[] indices;
		int[] viewport = new int[4];

		protected override void OnLoad()
		{
			timer = new Stopwatch();
			timer.Start();

			float x = 384f;
			float y = 400f;
			float w = 512f;
			float h = 256f;

			vertices = new float[]
			{
				x, y+h, 0f,		1f, 0f, 0f, 1f, // Vertex 0
				x+w, y+h, 0f,	0f, 1f, 0f, 1f, // Vertex 1
				x+w, y, 0f,		0f, 0f, 1f, 1f, // Vertex 2
				x, y, 0f,		0f, 1f, 0f, 1f, // Vertex 3
			};

			indices = new uint[]
			{
				0, 1, 2, // Triangle 1
				0, 2, 3 // Triangle 2
			};

			GL.GetInteger(GetPName.Viewport, viewport); 

			GL.ClearColor(0.3f, 0.4f, 0.5f, 1.0f);

			Console.WriteLine("Bind Vertex Buffer");
			this.vertexBufferObject = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBufferObject);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StreamDraw);

			this.vertexArrayObject = GL.GenVertexArray();
			GL.BindVertexArray(this.vertexArrayObject);
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBufferObject);

			// Positional Data
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 7 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			// Vertex Colour
			GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 7 * sizeof(float), 3 * sizeof(float));
			GL.EnableVertexAttribArray(1);

			Console.WriteLine("Bind Index Buffer");
			// Indices
			this.elementBufferObject =  GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.elementBufferObject);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

			string vertexShaderCode =
				@"	#version 330
					layout (location=0) in vec3 aPosition;
					layout (location=1) in vec3 aColour;

					uniform vec2 ViewportSize;
					uniform mat4 model;
					uniform mat4 view;
					uniform mat4 projection;

					out vec3 ourColour;

					void main()
					{
						float nx = aPosition.x / ViewportSize.x * 2 - 1;
						float ny = aPosition.y / ViewportSize.y * 2 - 1;

						gl_Position = vec4(nx, ny, 0, 1.0f) * model * view * projection;

						ourColour = aColour;
					}";

			string fragmentShaderCode =
				@"	#version 330
					out vec4 pixelColor;

					uniform vec4 timeColour;

					in vec3 ourColour;

					void main()
					{
						pixelColor = vec4(timeColour); //vec4(0.8f, 0.8f, 0.1f, 1.0f);
					}";

			Console.WriteLine("Compiling Vertex Shader");
			int vertexShaderObject = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexBufferObject, vertexShaderCode);
			GL.CompileShader(vertexShaderObject);

			Console.WriteLine("Compiling Fragment Shader");
			string vertexShaderInfo = GL.GetShaderInfoLog(vertexShaderObject);
			if (vertexShaderInfo != String.Empty)
			{
				Console.WriteLine("Vertex: \n" + vertexShaderInfo);
			}
			else
			{
				Console.WriteLine("Done");
			}

			int fragmentShaderObject = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShaderObject, fragmentShaderCode);
			GL.CompileShader(fragmentShaderObject);

			string fragmentShaderInfo = GL.GetShaderInfoLog(fragmentShaderObject);
			if (fragmentShaderInfo != String.Empty)
			{
				Console.WriteLine("Fragment: \n" + fragmentShaderInfo);
			}
			else
			{
				Console.WriteLine("Done");
			}


			this.shaderProgramObject = GL.CreateProgram();
			GL.AttachShader(this.shaderProgramObject, vertexShaderObject);
			GL.AttachShader(this.shaderProgramObject, fragmentShaderObject);
			GL.LinkProgram(this.shaderProgramObject);

			GL.DetachShader(this.shaderProgramObject, vertexShaderObject);
			GL.DetachShader(this.shaderProgramObject, fragmentShaderObject);
			GL.DeleteShader(vertexShaderObject);
			GL.DeleteShader(fragmentShaderObject);

			GL.GetInteger(GetPName.Viewport, viewport);

			GL.UseProgram(this.shaderProgramObject);
			int viewportSizeUniformLocation = GL.GetUniformLocation(this.shaderProgramObject, "ViewportSize");
			GL.Uniform2(viewportSizeUniformLocation, new OpenTK.Mathematics.Vector2((float) viewport[2], (float) viewport[3]));
			GL.UseProgram(0);

			this.IsVisible = true;

			base.OnLoad();
		}


		// Is executed every frame
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);

			//Console.WriteLine("Time: " + Time.time + "\ndelta: " + Time.deltaTime);

			if (KeyboardState.IsKeyDown(Keys.Escape))
			{
				Close();
			}

			if (KeyboardState.IsKeyDown(Keys.D))
			{
				Console.WriteLine("Time: " + Time.time + "\ndelta: " + Time.deltaTime);
			}
		}

		protected override void OnRenderFrame(FrameEventArgs args)
		{
			GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.UseProgram(this.shaderProgramObject);

			double timeValue = timer.Elapsed.TotalSeconds;
			int timeColorLocation = GL.GetUniformLocation(this.shaderProgramObject, "timeColour");
			float scalar = MathF.Sin((float)Time.time) / 2.0f + 0.5f;
			//Console.WriteLine(scalar);

			GL.Uniform4(timeColorLocation, 0, scalar, 0, 1f);

			Matrix4 viewportMatrix = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
			Matrix4 projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), (float)(viewport[2] / viewport[3]), 0.2f, 100.0f);
			Matrix4 modelMatrix = Matrix4.Identity * Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(180.0f * scalar));

			int loc1 = GL.GetUniformLocation(this.shaderProgramObject, "model");
			GL.UniformMatrix4(loc1, true, ref modelMatrix);

			int loc2 = GL.GetUniformLocation(this.shaderProgramObject, "view");
			GL.UniformMatrix4(loc2, true, ref viewportMatrix);

			int loc3 = GL.GetUniformLocation(this.shaderProgramObject, "projection");
			GL.UniformMatrix4(loc3, true, ref projectionMatrix);

			GL.BindVertexArray(this.vertexArrayObject);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.elementBufferObject);

			GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

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
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			GL.DeleteBuffer(this.elementBufferObject);

			GL.BindVertexArray(0);
			GL.DeleteVertexArray(this.vertexArrayObject);

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.DeleteBuffer(this.vertexBufferObject);

			GL.UseProgram(0);
			GL.DeleteProgram(this.shaderProgramObject);
			base.OnUnload();
		}
	}
}
