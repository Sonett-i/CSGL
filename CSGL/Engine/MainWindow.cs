using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Logging;
using CSGL.Engine.Shaders;

namespace CSGL
{
	public class MainWindow : GameWindow
	{

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
					Vsync = VSyncMode.On,
				})
		{
			this.Title = $"OpenGL";
			this.CenterWindow();
			InitializeWindow();
		}

		

		void InitializeWindow()
		{
			int[] viewport = new int[4];
			ShaderManager.CompileShaders();
		}

		protected override void OnLoad()
		{
			this.IsVisible = true; // make openGL window visible

			Scene debugScene = new DebugScene("debug");

			debugScene.Start();
			base.OnLoad();
		}

		protected override void OnMouseMove(MouseMoveEventArgs e)
		{

			base.OnMouseMove(e);
		}
		

		private void FixedUpdate()
		{

		}

		// Is executed before render frame
		protected override void OnUpdateFrame(FrameEventArgs e)
		{

			base.OnUpdateFrame(e);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			this.Context.SwapBuffers();
			GL.Finish();

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
			ShutDown();
			base.OnUnload();
		}

		void ShutDown()
		{
			Log.Default("CSGL Shut down");
		}
	}
}