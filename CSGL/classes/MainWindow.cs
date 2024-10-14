using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using System.Runtime.CompilerServices;

#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace CSGL
{

	// http://dreamstatecoding.blogspot.com/2017/01/opengl-4-with-opentk-in-c-part-2.html

	// http://dreamstatecoding.blogspot.com/2017/02/opengl-4-with-opentk-in-c-part-6.html HERE
	public class MainWindow : GameWindow
	{
		private int vertexBufferObject;
		private int vertexArrayObject;
		private int shaderProgramObject;
		private int elementBufferObject;

		ShaderProgram? shaderProgram;
		private List<RenderObject> renderObjects = new List<RenderObject>();

		string windowName;
		string filePath = Environment.CurrentDirectory + "\\Resources\\Shaders\\";

		private string vertexShader = "testmatrix.vert";
		private string fragmentShader = "simplePipe.frag";

		int[] viewport = new int[4];

		public MainWindow(int width, int height, string title) :
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
			this.windowName = title + ": OpenGL Version: " + GL.GetString(StringName.Version);
			this.Title = windowName;
			this.CenterWindow();
		}

		void HandleKeyboard()
		{
			if (KeyboardState.IsKeyDown(Keys.Escape))
			{
				Close();
			}

			if (KeyboardState.IsKeyDown(Keys.D))
			{
				Console.WriteLine(modelView.ToString());
			}
		}

		void InitializeShaders()
		{
			shaderProgram = new ShaderProgram(@filePath + vertexShader, @filePath + fragmentShader);
			shaderProgram = new ShaderProgram(@filePath + vertexShader, @filePath + fragmentShader);
		}

		void InitializeObjects()
		{
			Vertex[] vertices = ObjectFactory.CreateSolidCube(0.2f, Color4.HotPink);

			
			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
			GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
			GL.Enable(EnableCap.DepthTest);
		}

		protected override void OnLoad()
		{
			Console.WriteLine(filePath);

			InitializeShaders();
			InitializeObjects();

			GL.GenVertexArrays(1, out int _vertexArray);
			GL.BindVertexArray(_vertexArray);

			this.IsVisible = true;

			GL.GetInteger(GetPName.Viewport, viewport);


			base.OnLoad();
		}

		private Matrix4 modelView;

		// Is executed every frame
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			float t = Time.time * 0.5f;
			Matrix4 xR = Matrix4.CreateRotationX(t);
			Matrix4 yR = Matrix4.CreateRotationY(t);
			Matrix4 zR = Matrix4.CreateRotationZ(t);

			modelView = zR * yR * xR;


			HandleKeyboard();

			base.OnUpdateFrame(e);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			Title = windowName + $" (Vsync: {VSync}) FPS: {1f / e.Time:0} : Time {Time.time.ToString("0.00")} : Delta: {Time.deltaTime.ToString("0.00")}";

			// Clear BG Colour
			Color4 backColour = new Color4(0.1f, 0.1f, 0.3f, 1.0f);

			GL.ClearColor(backColour);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			
			GL.UseProgram(shaderProgram.ShaderProgramHandle);

			
			this.Context.SwapBuffers();
			base.OnRenderFrame(e);
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			GL.Viewport(0, 0, e.Width, e.Height);
			base.OnResize(e);
		}

		protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
		{
			GL.Viewport(0, 0, e.Width, e.Height);

			base.OnFramebufferResize(e);
		}

		protected override void OnUnload()
		{
			foreach (RenderObject renderObject in renderObjects)
			{
				renderObject.Dispose();
			}

			base.OnUnload();
		}
	}
}

/*	To-do
 *	dreamstatecoding
 *		http://dreamstatecoding.blogspot.com/2017/01/opengl-4-with-opentk-in-c-part-2.html
 * 
 */

/*
 * 
 * 
 */
