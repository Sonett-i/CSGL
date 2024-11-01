using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

#pragma warning disable 8618

namespace CSGL
{
	public static class Input
	{
		public static Mouse Mouse = new Mouse();

		public static KeyboardState KeyboardState;
		public static MouseState MouseState;

		public static void Update(KeyboardState keyboardState, MouseState mouseState)
		{
			KeyboardState = keyboardState;
			MouseState = mouseState;

			Mouse.Update(mouseState);
		}

		public static unsafe void SetMousePosition(Window* window,int x, int y)
		{
			GLFW.SetCursorPos(window, x, y);
		}

		public static float GetAxisRaw(string axis)
		{
			if (axis == "Horizontal")
			{
				if (KeyboardState.IsKeyDown(Keys.A))
				{
					return -1.0f;
				}

				if (KeyboardState.IsKeyDown(Keys.D))
				{
					return 1.0f;
				}
			}

			if (axis == "Vertical")
			{
				if (KeyboardState.IsKeyDown(Keys.W))
				{
					return 1.0f;
				}

				if ( KeyboardState.IsKeyDown(Keys.S))
				{
					return -1.0f;
				}
			}

			return 0;
		}
	}

	public class Mouse
	{
		public Vector2 Position;
		public bool LeftButtonPressed = false;
		public bool RightButtonPressed = false;
		public bool LeftButtonDown = false;
		public bool RightButtonDown = false;

		public void Update(MouseState mouseState)
		{
			this.Position = new Vector2(mouseState.Position.X, Viewport.Height - mouseState.Position.Y);
		}

		//	Config

		public static float Sensitivity = 0.1f;
	}
}
