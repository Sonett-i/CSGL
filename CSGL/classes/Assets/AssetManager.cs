using System;

namespace CSGL
{
	public static class AssetManager
	{
		public static List<Asset> assetList = new List<Asset>();

		public static void Initialize()
		{
			ShaderManager.Initialize();
			ModelManager.Initialize();

			string[] folders = Directory.GetDirectories(EditorConfig.AssetDirectory);
			foreach (string folder in folders)
			{
				string[] subDir = Directory.GetDirectories(folder);
				
			}
		}
	}
}
