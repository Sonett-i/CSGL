using System;
using CSGL;
using Import;
using Import.Extensions;
using Logging;

namespace Interface
{
	public class Game
	{
		static void Init()
		{
			Log.Init(Import.ManagedDirectories.LogDirectory);
			LoadConfig();
			Manifest.UpdateFileManifest();
		}

		static void LoadConfig()
		{
			INI? config = Import.Config.Import();

			if (config != null)
				CSGL.Config.Set(config);
		}

		static void UpdateManifest()
		{

		}

		public static int Main(string[] args)
		{
			Init();
			
			Log.Default($"CSGL: {CSGL.EngineConfig.Name}\n\tVersion: {EngineConfig.Version}");

			
			// GPR202 Assessment 3
			using (MainWindow game = new MainWindow(CSGL.WindowConfig.Width, CSGL.WindowConfig.Height, $"{CSGL.EngineConfig.Name}:{CSGL.EngineConfig.Name}"))
			{
				game.Run();
			}
			Log.Default("Graceful");
			return (0);
		}
	}
}