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
		private List<RenderObject> renderObjects = new List<RenderObject>();

		RenderObject? quad;
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
			WindowConfig.Name = title + ": OpenGL Version: " + GL.GetString(StringName.Version);
			this.Title = WindowConfig.Name;
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

		void InitializeWindow()
		{
			GL.GetInteger(GetPName.Viewport, WindowConfig.Viewport);
			Color4 backColour = new Color4(0.1f, 0.1f, 0.3f, 1.0f);

			GL.ClearColor(backColour);
		}

		void InitializeShaders()
		{
			ShaderManager.Initialize();
		}

		void InitializeObjects()
		{
			quad = ObjectFactory.CreateQuad(Vector3.Zero, 1f, 1f, ShaderManager.GetShader("shader"));

			renderObjects.Add(quad);
			//renderObjects.Add(ObjectFactory.CreateTriangle(new Vector3(0.25f, 0.25f, 0.0f), 1f, shaderProgram));
		}

		protected override void OnLoad()
		{
			Log.Default($"Initializing {WindowConfig.Name}");

			InitializeWindow();
			InitializeShaders();
			InitializeObjects();

			this.IsVisible = true;
			base.OnLoad();
		}

		private Matrix4 modelView;

		// Is executed every frame
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			Time.Tick();
			HandleKeyboard();

			base.OnUpdateFrame(e);
		}

		private void PollWindow(float time)
		{
			//Console.WriteLine(time + " " + Time.time);

			if (Time.time >= Time.NextPoll)
			{
				Title = WindowConfig.Name + $" (Vsync: {VSync}) FPS: {1f / time:0} : Time {Time.time.ToString("0.00")} : Delta: {Time.deltaTime.ToString("0.00")}";
				Time.NextPoll = Time.time + Time.PollInterval;
			}
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			PollWindow((float)e.Time);
			//Title = windowName + $" (Vsync: {VSync}) FPS: {1f / e.Time:0} : Time {Time.time.ToString("0.00")} : Delta: {Time.deltaTime.ToString("0.00")}";

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			foreach (RenderObject renderObject in renderObjects)
			{
				renderObject.Render();
			}
			
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
