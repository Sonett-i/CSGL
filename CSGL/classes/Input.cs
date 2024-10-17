using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CSGL
{
	public static class Input
	{
		public static Mouse Mouse = new Mouse();
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

		public void Update(Vector2 position, bool leftMouse, bool leftMouseDown, bool rightMouse, bool rightMouseDown)
		{
			this.Position = new Vector2(position.X, Viewport.Height - position.Y);
			this.LeftButtonPressed = leftMouse;
			this.LeftButtonDown = leftMouseDown;
			this.RightButtonPressed = rightMouse;
			this.RightButtonDown = rightMouseDown;
		}
	}
}
