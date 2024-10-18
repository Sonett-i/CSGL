using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

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

		public static Vector2 GetAxisRaw(string axis)
		{
			Vector2 result = new Vector2(0, 0);

			if (axis == "Horizontal")
			{
				if (KeyboardState.IsKeyDown(Keys.A))
				{
					result.X = -1.0f;
				}

				if (KeyboardState.IsKeyDown(Keys.D))
				{
					result.X += 1.0f;
				}
				return result;
			}

			if (axis == "Vertical")
			{
				if (KeyboardState.IsKeyDown(Keys.W))
				{
					result.Y = 1.0f;
				}

				if ( KeyboardState.IsKeyDown(Keys.S))
				{
					result.Y = -1.0f;
				}

				return result;
			}

			return result;
		}
	}

	public class Mouse
	{
		public Vector2 Position;
		public bool LeftButtonPressed = false;
		public bool RightButtonPressed = false;
		public bool LeftButtonDown = false;
		public bool RightButtonDown = false;

		public Mouse()
		{

		}

		public void Update(MouseState mouseState)
		{
			this.Position = new Vector2(mouseState.Position.X, Viewport.Height - mouseState.Position.Y);
		}
	}
}
