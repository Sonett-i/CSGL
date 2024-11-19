using System;
using CSGL;
using ContentPipeline;
using ContentPipeline.Extensions;
using Logging;

namespace Interface
{
	public class Game
	{
		static void Init()
		{
			Log.Init(ManagedDirectories.LogDirectory);
			LoadConfig();
			Log.Default($"CSGL: {CSGL.EngineConfig.Name}\nVersion: {EngineConfig.Version}");

			Manifest.UpdateFileManifest();
		}

		static void LoadConfig()
		{
			INI? config = ContentPipeline.Config.Import();

			if (config != null)
			{
				CSGL.Config.Set(config);
				ContentPipeline.ManagedFormats.Configure(config);
			}
				
		}

		static void UpdateManifest()
		{

		}

		public static int Main(string[] args)
		{
			Init();
			
			

			
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