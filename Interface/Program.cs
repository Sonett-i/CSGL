using System;
using CSGL;

namespace Interface
{
	public class Game
	{
		public static int GameID = 0;

		public static int Main(string[] args)
		{
			LoadConfig.ImportFromJson();

			// GPR202 Assessment 2
			using (MainWindow game = new MainWindow(WindowConfig.Width, WindowConfig.Height, WindowConfig.Name + ": A2"))
			{
				game.Run();
			}
			return (0);
		}
	}
}