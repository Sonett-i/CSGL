using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CSGL.classes
{
	public class Window : GameWindow
	{
		public Window(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title }) { }

		float[] vertices =
		{
			-0.5f, -0.5f, 0.0f, // Bottom Left
			0.5f, -0.5f, 0.0f, // Bottom Right
			0.0f, 0.5f, 0.0f // Top
		};

		int VertexBufferObject;
		Shader shader;

		string filePath = Environment.CurrentDirectory;


		// Is executed every frame
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);

			if (KeyboardState.IsKeyDown(Keys.Escape))
			{
				Close();
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
			base.OnLoad();

			Console.WriteLine("OpenGL: " + ClientInfo());
			GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

			VertexBufferObject = GL.GenBuffer();

			GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

			shader = new Shader(vertices, filePath + "\\Resources\\Shaders\\" + "shader.vert", filePath + "\\Resources\\Shaders\\" + "shader.frag");

			int VertexArrayObject = GL.GenVertexArray();
			GL.BindVertexArray(VertexArrayObject);

			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			shader.Use();
		}

		protected override void OnUnload()
		{
			base.OnUnload();

			shader.Dispose();
		}

		// Render frame
		protected override void OnRenderFrame(FrameEventArgs args)
		{
			base.OnRenderFrame(args);

			GL.Clear(ClearBufferMask.ColorBufferBit);

			shader.Use();
			GL.BindVertexArray(VertexBufferObject);
			GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

			// Code here

			SwapBuffers();
		}

		protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
		{
			base.OnFramebufferResize(e);

			GL.Viewport(0, 0, e.Width, e.Height);
		}
	}
}

/*	Reference:
 *		https://opentk.net/learn/chapter1/2-hello-triangle.html?tabs=onload-opentk4%2Conrender-opentk4%2Cresize-opentk4
 */