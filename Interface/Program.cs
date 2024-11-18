using System;
using CSGL;

namespace Interface
{
	public class Game
	{
		public static int GameID = 0;

		public static int Main(string[] args)
		{
			int width = 1024;
			int height = 768;
			string WindowTitle = "CSGL: 3";


			// GPR202 Assessment 3
			using (MainWindow game = new MainWindow(width, height, WindowTitle))
			{
				game.Run();
			}
			return (0);
		}
	}
}