using System;
using CSGL;
using CSGL.classes;
using MathU;

namespace Program
{
	class Application
	{
		public static int Main(string[] args)
		{
			using (Window window = new Window(800, 600, "OpenTK"))
			{
				window.Run();
			}
			return (0);
		}
	}
}