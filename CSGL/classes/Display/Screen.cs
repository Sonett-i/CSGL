using System;
using System.Numerics;

namespace CSGL
{
	public static class Viewport
	{
		public static int x;
		public static int y;

		public static int Width;
		public static int Height;

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
	}
}
