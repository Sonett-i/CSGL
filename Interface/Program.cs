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
			Colour red = new Colour(1, 2, 3, 5);
			
			Console.WriteLine(red.ToString());
			using (Window window = new Window(800, 600, "OpenTK"))
			{
				window.Run();
			}
			return (0);
		}
	}
}