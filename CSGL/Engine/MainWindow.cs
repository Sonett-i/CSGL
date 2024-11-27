﻿using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Logging;
using CSGL.Engine;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CSGL
{
	public class MainWindow : GameWindow
	{
		public static MainWindow Instance = null!;

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
					Vsync = VSyncMode.Off,
				})
		{
			this.Title = $"OpenGL";
			this.CenterWindow();
			Instance = this;
			InitializeWindow();
		}

		void InitializeWindow()
		{
			int[] viewport = new int[4];

			UpdateFrequency = WindowConfig.TargetFrameRate;

			Config.SetOpenGLConfig();

			ShaderManager.CompileShaders();
			//TextureManager.CompileTextures();

			Color4 backColour = new Color4(EngineConfig.BackdropColour.X, EngineConfig.BackdropColour.Y, EngineConfig.BackdropColour.Z, 1.0f);
			GL.ClearColor(backColour);

			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.StencilTest);
			GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
		}

		protected override void OnLoad()
		{
			this.IsVisible = true; // make openGL window visible

			Scene debugScene = new TerrainTest("debug");

			debugScene.Start();
			SceneManager.ActiveScene = debugScene;

			base.OnLoad();
		}

		void UpdateCursor()
		{
			if (MouseState.IsButtonDown(MouseButton.Right))
				this.CursorState = CursorState.Grabbed;
			else if (MouseState.IsButtonReleased(MouseButton.Right))
				this.CursorState = CursorState.Normal;
			else
				this.CursorState = CursorState.Normal;
		}

		protected override void OnMouseMove(MouseMoveEventArgs e)
		{
			if (this.IsFocused)
			{
				UpdateCursor();

				Input.MouseMove(e);
			}

			base.OnMouseMove(e);
		}
		

		private void FixedUpdate()
		{
			SceneManager.ActiveScene.FixedUpdate();
		}

		// Is executed before render frame
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			UpdateCursor();

			Time.accumulatedTime += Time.deltaTime;

			Input.Update(KeyboardState, MouseState);

			SceneManager.ActiveScene.Update();

			while (Time.accumulatedTime >= Time.fixedDeltaTime)
			{
				FixedUpdate();
				Time.accumulatedTime -= Time.fixedDeltaTime;
			}
			
			base.OnUpdateFrame(e);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			Time.Update(e);
			
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
			GL.StencilFunc(StencilFunction.Always, 1, 0xFF);
			GL.StencilMask(0xFF);

			SceneManager.ActiveScene.Render();
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
			Log.Default("CSGL Shut down");
			SceneManager.ActiveScene.Unload();
			base.OnUnload();
		}

		public void Shutdown()
		{
			Close();
		}
	}
}