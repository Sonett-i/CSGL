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

				Asset? asset = AssetManager.Import(file);

				if (asset != null)
				{
					try
					{
						if (!FileManifest[asset.Type].ContainsKey(asset.ID))
						{
							FileManifest[asset.Type].Add(asset.ID, asset);
							//Log.Info($"{asset.Type}:{asset.Name} added to manifest: {asset.ToString()}");
						}
						else
						{
							throw new Exception("File already exists in manifest");
						}
					}
					catch (Exception ex)
					{
						Log.Error(ex.Message);
					}
				}
			}
			
			Log.Default($"Manifest Updated: {Scan.scannedFiles.Count} files found\n{Info()}");
		}

		public static T? GetAsset<T>(string name) where T : Asset
		{
			Type type = typeof(T);
			AssetType aType = AssetManager.GetAssetFromType(type);

			foreach (KeyValuePair<Guid, Asset> kvp in FileManifest[aType])
			{
				if (kvp.Value.Name == name)
				{
					return kvp.Value as T;
				}
			}

			throw new Exception("File not found");
		}

		public static string Info()
		{
			string output = "";

			foreach (KeyValuePair<AssetType, Dictionary<Guid, Asset>> kvp in FileManifest)
			{
				output += $"{kvp.Key.ToString()}: {kvp.Value.Count}\n";
			}

			return output;
		}
	}
}
