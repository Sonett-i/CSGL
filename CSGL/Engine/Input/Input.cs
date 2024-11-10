using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
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


			Mouse.Update(mouseState);
		}

		public static void MouseMove(MouseMoveEventArgs e)
		{
			Mouse.Delta = e.Delta;
			Mouse.Position = e.Position;
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

		public static float GetArrowInput(string axis)
		{
			if (axis == "Horizontal")
			{
				if (KeyboardState.IsKeyDown(Keys.Left))
				{
					return -1.0f;
				}

				if (KeyboardState.IsKeyDown(Keys.Right))
				{
					return 1.0f;
				}
			}

			if (axis == "Vertical")
			{
				if (KeyboardState.IsKeyDown(Keys.Up))
				{
					return 1.0f;
				}

				if (KeyboardState.IsKeyDown(Keys.Down))
				{
					return -1.0f;
				}
			}

			return 0;
		}

		public static float GetNumpadInput(string axis)
		{
			if (axis == "Horizontal")
			{
				if (KeyboardState.IsKeyDown(Keys.KeyPad4))
				{
					return -1.0f;
				}

				if (KeyboardState.IsKeyDown(Keys.KeyPad6))
				{
					return 1.0f;
				}
			}

			if (axis == "Vertical")
			{
				if (KeyboardState.IsKeyDown(Keys.KeyPad8))
				{
					return 1.0f;
				}

				if (KeyboardState.IsKeyDown(Keys.KeyPad2))
				{
					return -1.0f;
				}
			}

			if (axis == "Diagonal")
			{
				if (KeyboardState.IsKeyDown(Keys.KeyPad9))
				{
					return 1.0f;
				}

				if (KeyboardState.IsKeyDown(Keys.KeyPad1))
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
		public Vector2 Delta = Vector2.Zero;

		public void Update(MouseState mouseState)
		{
			this.Position = new Vector2(mouseState.Position.X, Viewport.Height - mouseState.Position.Y);
			this.RightButtonPressed = mouseState.IsButtonDown(MouseButton.Right);
		}

		//	Config

		public static float Sensitivity = 0.1f;
	}
}
