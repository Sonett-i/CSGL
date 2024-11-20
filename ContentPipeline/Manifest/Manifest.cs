using System;
using ContentPipeline.Utilities;
using Logging;

namespace ContentPipeline
{
	public class Manifest
	{
		public static Dictionary<AssetType, Dictionary<Guid, Asset>> FileManifest = Enum.GetValues(typeof(AssetType))
			.Cast<AssetType>()
			.ToDictionary(
				type => type,
				type => new Dictionary<Guid, Asset>()
			);

		public static void UpdateFileManifest()
		{
			Log.Default($"Updating File Manifest");

			// scan all files first
			Scan.ScanFolders();

			// then add to File Manifest
			for (int i = 0; i < Scan.scannedFiles.Count; i++)
			{
				string file = Scan.scannedFiles[i];

				Log.Default(i + " " + file);
				Asset? asset = AssetManager.Import(file);

				if (asset != null)
				{
					try
					{
						FileManifest[asset.Type].Add(asset.ID, asset);
						Log.Info($"{asset.Type}:{asset.Name} added to manifest: {asset.ToString()}");
					}
					catch (Exception ex)
					{
						Log.Error(ex.Message);
					}
				}
			}
			
			Log.Default($"Manifest Updated: {Scan.scannedFiles.Count} files found");
		}

		public static string Info()
		{
			string output = "Shaders: 0" +
				"Textures: 0" +
				"Models: 0" +
				"Materials: 0";
			return output;
		}
	}
}
