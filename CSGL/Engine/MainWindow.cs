using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace CSGL
{
	public class MainWindow : GameWindow
	{
		public MainWindow(int width, int height, string title) : base(GameWindowSettings.Default,
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
				Vsync = VSyncMode.On
			})
		{
			this.Title = "OpenGL";
			this.CenterWindow();
		}

		protected override void OnLoad()
		{
			this.IsVisible = true;
			base.OnLoad();
		}

		protected override void OnUpdateFrame(FrameEventArgs args)
		{
			base.OnUpdateFrame(args);
		}

		protected override void OnRenderFrame(FrameEventArgs args)
		{
			base.OnRenderFrame(args);
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			GL.Viewport(0, 0, e.Width, e.Height);
			base.OnResize(e);
		}

		protected override void OnUnload()
		{
			base.OnUnload();
		}
	}
}
