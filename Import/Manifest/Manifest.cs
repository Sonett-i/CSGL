using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logging;

namespace Import
{
	public class Manifest
	{
		public static Dictionary<int, Asset> FileManifest = new Dictionary<int, Asset>();
		public static void UpdateFileManifest()
		{
			Log.Default($"Updating File Manifest");
			
			Scan.ScanFolders();
			
			for (int i = 0; i < Scan.scannedFiles.Count; i++)
			{
				string file = Scan.scannedFiles[i];

				Log.Default(i + " " + file);
				Asset asset = Asset.Import(file);

				if (asset != null)
				{
					FileManifest.Add(i, asset);
				}
			}
			// scan all files first


			// then add to File Manifest
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
