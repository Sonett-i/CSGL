using System;
using CSGL;

namespace Program
{
	class Application
	{
		public static int Main(string[] args)
		{
			// Assessment
			using (MainWindow game = new MainWindow(1280, 768, "Game"))
			{
				game.Run();
			}

			/*
			using (Game game = new Game(1280, 768, "Game"))
			{
				game.Run();
			}

			
			using (Window window = new Window(800, 600, "OpenTK"))
			{
				window.Run();
			}
			*/


			return (0);
		}
	}
}