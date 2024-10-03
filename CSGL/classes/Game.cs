using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CSGL
{
	public class Game : GameWindow
	{
		private int vertexBufferObject;
		private int vertexArrayObject;
		private int shaderProgramObject;
		private int elementBufferObject;

		float[] vertices = new float[]
		{
			// position				colour
			0.5f,   0.5f,   0.0f,   1.0f, 0.0f, 0.0f, 1.0f,	//	Vertex 0 - top right
			0.5f,   -0.5f,  0.0f,   0.0f, 1.0f, 0.0f, 1.0f,	//	Vertex 1 - bottom right
			-0.5f, -0.5f,	0.0f,   1.0f, 1.0f, 0.0f, 1.0f, //	Vertex 2 - bottom left
			-0.5f,  0.5f,  0.0f,   0.0f, 0.0f, 1.0f, 1.0f  //	Vertex 3 - top left
		};

		uint[] indices = new uint[]
		{
			0, 1, 3, // Triangle 1
			1, 2, 3 // Triangle 2
		};

		public Game(int width, int height, string title) : 
			base(GameWindowSettings.Default, 
				new NativeWindowSettings()
				{
					Title = title,
					ClientSize = new Vector2i(width, height),
					WindowBorder = WindowBorder.Fixed,
					StartVisible = true,
					StartFocused = true,
					API = ContextAPI.OpenGL,
					Profile = ContextProfile.Core,
					APIVersion = new Version(4, 1)
				})
		{

			this.CenterWindow();
		}

		protected override void OnLoad()
		{
			base.OnLoad();
			GL.ClearColor(0.3f, 0.4f, 0.5f, 1.0f);

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

			// Indices
			this.elementBufferObject =  GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.elementBufferObject);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

			string vertexShaderCode =
				@"	#version 330
					layout (location=0) in vec3 aPosition;
					layout (location=1) in vec3 aColour;

					out vec3 ourColour;

					void main()
					{
						gl_Position = vec4(aPosition, 1.0f);
						ourColour = aColour;
					}";

			string fragmentShaderCode =
				@"	#version 330
					out vec4 pixelColor;

					in vec3 ourColour;

					void main()
					{
						pixelColor = vec4(ourColour, 1.0f); //vec4(0.8f, 0.8f, 0.1f, 1.0f);
					}";

			int vertexShaderObject = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexBufferObject, vertexShaderCode);
			GL.CompileShader(vertexShaderObject);

			string vertexShaderInfo = GL.GetShaderInfoLog(vertexShaderObject);
			if (vertexShaderInfo != String.Empty)
			{
				Console.WriteLine("Vertex: " + vertexShaderInfo);
			}

			int fragmentShaderObject = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShaderObject, fragmentShaderCode);
			GL.CompileShader(fragmentShaderObject);

			string fragmentShaderInfo = GL.GetShaderInfoLog(fragmentShaderObject);
			if (fragmentShaderInfo != String.Empty)
			{
				Console.WriteLine("Fragment: " + fragmentShaderInfo);
			}

			this.shaderProgramObject = GL.CreateProgram();
			GL.AttachShader(this.shaderProgramObject, vertexShaderObject);
			GL.AttachShader(this.shaderProgramObject, fragmentShaderObject);
			GL.LinkProgram(this.shaderProgramObject);

			GL.DetachShader(this.shaderProgramObject, vertexShaderObject);
			GL.DetachShader(this.shaderProgramObject, fragmentShaderObject);
			GL.DeleteShader(vertexShaderObject);
			GL.DeleteShader(fragmentShaderObject);
		}


		protected override void OnUpdateFrame(FrameEventArgs args)
		{
			base.OnUpdateFrame(args);
		}

		protected override void OnRenderFrame(FrameEventArgs args)
		{
			GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.UseProgram(this.shaderProgramObject);

			GL.BindVertexArray(this.vertexArrayObject);
			//GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

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
			base.OnFramebufferResize(e);

			GL.Viewport(0, 0, e.Width, e.Height);
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
