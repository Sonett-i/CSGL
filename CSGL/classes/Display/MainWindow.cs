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
	public class MainWindow : GameWindow
	{
		public List<GameObject> gameObjects = new List<GameObject>();		

		public Scene scene = new Scene("Default");
		private float timeInterval = 0.0f;
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
					APIVersion = new Version(4, 1),
					Vsync = WindowConfig.VSyncMode,
				})
		{
			WindowConfig.Name = title + ": OpenGL Version: " + GL.GetString(StringName.Version);
			this.Title = WindowConfig.Name;
			this.CenterWindow();
		}

		void InitializeWindow()
		{
			int[] viewport = new int[4];
			GL.GetInteger(GetPName.Viewport, viewport);
			Viewport.Set(viewport);

			GL.Enable(EnableCap.DepthTest);

			Color4 backColour = new Color4(0.1f, 0.1f, 0.3f, 1.0f);

			GL.ClearColor(backColour);
		}



		protected override void OnLoad()
		{
			Log.Default($"Initializing {WindowConfig.Name}");

			InitializeWindow();
			AssetManager.Initialize();
			scene.Start();
			this.IsVisible = true;
			base.OnLoad();
		}

		void HandleKeyboard()
		{
			if (KeyboardState.IsKeyDown(Keys.Escape))
			{
				Close();
			}

			if (KeyboardState.IsKeyReleased(Keys.R))
			{
				
				//ShaderManager.HotReload();
			}
		}

		private void FixedUpdate()
		{
			scene.FixedUpdate();
		}

		// Is executed before render frame
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			Time.Tick();
			Input.Update(KeyboardState, MouseState);

			HandleKeyboard();

			scene.Update();

			timeInterval += (float)e.Time;

			if (timeInterval >= WindowConfig.FixedInterval)
			{
				FixedUpdate();
				timeInterval = 0;
			}

			base.OnUpdateFrame(e);
		}

		private void PollWindow(float time)
		{
			//Console.WriteLine(time + " " + Time.time);

			if (Time.time >= Time.NextPoll)
			{
				Title = WindowConfig.Name + $" (Vsync: {VSync}) FPS: {1f / time:0} : Time {Time.time.ToString("0.00")} : Delta: {Time.deltaTime.ToString("0.00")} Mouse: ({Input.Mouse.Position.X}, {Input.Mouse.Position.Y})";
				Time.NextPoll = Time.time + Time.PollInterval;
			}
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			PollWindow((float)e.Time);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			scene.Render();
			
			
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
			foreach (RenderObject renderObject in scene.sceneObjects)
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
 *	// http://dreamstatecoding.blogspot.com/2017/01/opengl-4-with-opentk-in-c-part-2.html

	// http://dreamstatecoding.blogspot.com/2017/02/opengl-4-with-opentk-in-c-part-6.html HERE
 * 
 */

/*
 * 
 * 
 */
