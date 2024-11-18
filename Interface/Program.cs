using System;
using CSGL;
using Import;
using Import.Extensions;
using Logging;

namespace Interface
{
	public class Game
	{
		public static int GameID = 0;

		static void LoadConfig()
		{
			Log.Default("Importing config from ini");
			INI? config = Import.Config.Import();

			if (config != null)
				CSGL.Config.Set(config);
		}

		public static int Main(string[] args)
		{
			LoadConfig();
			
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