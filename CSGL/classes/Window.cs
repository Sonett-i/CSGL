﻿using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CSGL.classes
{
	public class Window : GameWindow
	{
		public Window(int width, int height, string title) :
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

		float[] vertices = new float[]
		{
			// position				colour
			-0.5f,  0.5f,   0.0f,   1.0f, 0.0f, 0.0f, 1.0f,	//	Vertex 0 - top right
			0.5f,   0.5f,  0.0f,    0.0f, 1.0f, 0.0f, 1.0f,	//	Vertex 1 - bottom right
			0.5f, -0.5f,   0.0f,    0.0f, 0.0f, 1.0f, 1.0f, //	Vertex 2 - bottom left
			-0.5f,  -0.5f,  0.0f,   1.0f, 1.0f, 0.0f, 1.0f  //	Vertex 3 - top left
		};

		uint[] indices = new uint[]
		{
			0, 1, 2, // Triangle 1
			0, 2, 3 // Triangle 2
		};

		int VertexBufferObject;
		Shader shader;

		string filePath = Environment.CurrentDirectory;


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

		public string ClientInfo()
		{
			string output = "OpenGL:" + GL.GetString(StringName.Version);

			output += "\nWidth: " + ClientSize.X + "\nHeight: " + ClientSize.Y;

			return output;
		}

		// When the window loads
		protected override void OnLoad()
		{
			Console.WriteLine("OpenGL: " + ClientInfo());
			GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

			VertexBufferObject = GL.GenBuffer();

			GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

			shader = new Shader(vertices, indices, filePath + "\\Resources\\Shaders\\" + "shader.vert", filePath + "\\Resources\\Shaders\\" + "shader.frag");

			int VertexArrayObject = GL.GenVertexArray();
			GL.BindVertexArray(VertexArrayObject);

			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			shader.Use();

			this.IsVisible = true;
			base.OnLoad();
		}

		protected override void OnUnload()
		{
			GL.BindVertexArray(0);
			shader.Dispose();

			base.OnUnload();

			
		}

		// Render frame
		protected override void OnRenderFrame(FrameEventArgs args)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			shader.Use();
			GL.BindVertexArray(VertexBufferObject);
			GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);


			SwapBuffers();
			base.OnRenderFrame(args);
		}

		protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
		{
			base.OnFramebufferResize(e);

			GL.Viewport(0, 0, e.Width, e.Height);
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			GL.Viewport(0, 0, e.Width, e.Height);
			base.OnResize(e);
		}
	}
}

/*	Reference:
 *		https://opentk.net/learn/chapter1/2-hello-triangle.html?tabs=onload-opentk4%2Conrender-opentk4%2Cresize-opentk4
 */