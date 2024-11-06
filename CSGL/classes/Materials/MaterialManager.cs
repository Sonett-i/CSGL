using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Reflection;

namespace CSGL
{
	public class MaterialManager
	{
		public static List<Material> Materials = new List<Material>();
		public static Material Default;
		public static void Import()
		{
			Log.Default("Importing Materials");
			string[] files = Directory.GetFiles(EditorConfig.MaterialDirectory);

			Log.Default($"Found {files.Length} model(s) in {EditorConfig.MaterialDirectory}");

			for (int i = 0; i < files.Length; i++)
			{
				string ext = Path.GetExtension(files[i]);
				string fileName = Path.GetFileName(files[i]).Replace(ext, "");

				if (ext == ".json")
				{
					Log.Default($"Importing {fileName}{ext}");

					Material mat = Material.LoadFromJson(files[i]);

					if (mat != null)
					{
						Materials.Add(mat);

						if (mat.Name == "default")
						{
							Default = mat;
						}
					}
				}
			}
		}

		public static Material GetMaterial(string name)
		{
			foreach (Material mat in Materials)
			{
				if (mat.Name == name)
					return mat;
			}

			return MaterialManager.Default;
		}
	}
}
