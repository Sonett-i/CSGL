using OpenTK.Mathematics;
using OpenTK;
using System;


namespace CSGL
{
	public static class Viewport
	{
		public static int x;
		public static int y;

		public static int Width;
		public static int Height;

		public static Vector2i CenterViewport; //	Center of OpenTK window
		public static Vector2i CenterScreen; //	Center of device screen

		public static float AspectRatio
		{
			get
			{
				return Width / Height;
			}
		}

		public static void Set(int[] viewport)
		{
			x = viewport[0];
			y = viewport[1];
			Width = viewport[2];
			Height = viewport[3];
		}

		public static void SetScreen(Vector2i Viewport, Vector2i Screen)
		{
			CenterScreen = Screen;
			CenterViewport = Viewport;
		}

		public static Vector2 ToScreenSpace(Vector2 screenSpace)
		{
			return ToScreenSpace(screenSpace.X, screenSpace.Y);
		}

		public static Vector2 ToScreenSpace(float x, float y)
		{
			float nx = (x / Width) * 2 - 1;
			float ny = (y / Height) * 2 - 1;

			return new Vector2(nx, ny);
		}

		public static Vector2i ToViewPort(Vector2 screenSpace)
		{
			Vector2i result = new Vector2i();

			

			return result;
		}
	}
}
