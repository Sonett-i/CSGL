using System;
using CSGL;

namespace Program
{
	class Application
	{
		public static int Main(string[] args)
		{
			// Assessment
			using (MainWindow game = new MainWindow(1280, 768, CSGL.WindowConfig.Name + ": A1"))
			{
				game.Run();
			}
			return (0);
		}
	}
}