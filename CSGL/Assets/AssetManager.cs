using System;
using System.Diagnostics;

namespace CSGL
{
	public static class AssetManager
	{
		public static List<Asset> assetList = new List<Asset>();
		public static List<Monobehaviour> GameObjects = new List<Monobehaviour>();

		public static List<string> assetPaths = new List<string>();

		public static void Initialize()
		{
			Resources.ImportShaders();
			Resources.ImportTextures();
			Resources.ImportMaterials();
			Resources.ImportModels();

			AssetManager.Import();
		}

		public static void Import()
		{
			Log.Default("Importing assets from: " + EditorConfig.AssetDirectory);
			string[] folders = Directory.GetDirectories(EditorConfig.AssetDirectory);

			foreach (string folder in folders)
			{
				Explore(folder);
			}
		}

		public static string Explore(string path)
		{
			string result = "";

			string[] subDirectory = Directory.GetDirectories(path);

			if (subDirectory.Length > 0)
			{
				for (int i = 0; i < subDirectory.Length; i++)
				{
					result = Explore(subDirectory[i]);
				}
			}

			string[] files = Directory.GetFiles(path);

			if (files.Length > 0)
			{
				for (int i = 0; i < files.Length; i++)
				{
					string fileName = Path.GetFileName(files[i]);
					string ext = Path.GetExtension(files[i]);

					if (ext == ".json")
					{
						Asset? asset = Asset.ImportFromJson(files[i]);
					}

					if (ext == ".jpg" || ext == ".png")
					{
						//Resources.T TextureManager.Import(files[i], fileName);
					}
				}
			}

			return result;
		}
	}
}
