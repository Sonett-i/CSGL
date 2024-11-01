using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;

namespace CSGL
{
	public static class WindowConfig
	{
		public static string Name = "GPR202";
		public static float FixedInterval = 0.02f;
		public static VSyncMode VSyncMode = VSyncMode.On;

		public static bool StickyMouse = true;
		public static bool CursorVisible = false;
	}
}
