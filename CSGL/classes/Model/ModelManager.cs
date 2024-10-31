using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CSGL;

#pragma warning disable 8603

namespace CSGL
{
	public static class ModelManager
	{
		public static List<Model> models = new List<Model>();

		public static void Import()
		{
			Log.Default("Importing Models");
			string[] files = Directory.GetFiles(EditorConfig.ModelDirectory);

			Log.Default($"Found {files.Length} model(s) in {EditorConfig.ModelDirectory}");

			for (int i = 0; i < files.Length; i++)
			{
				string ext = Path.GetExtension(files[i]);
				string fileName = Path.GetFileName(files[i]).Replace(ext, "");

                if (ext == ".obj")
                {
					Log.Default($"Importing {fileName}{ext}");
					Model model = OBJ.Import(File.ReadAllLines(EditorConfig.ModelDirectory + fileName + ext));
					models.Add(model);
					Log.Default($"Loaded {fileName + ext}: " + model.ToString() + "\n");
                }

				if (ext == ".fbx")
				{
					Log.Default($"Importing {fileName}{ext}");
					Model model = FBX.Import(File.ReadAllBytes(EditorConfig.ModelDirectory + fileName + ext));
				}
                
			}
		}

		public static Model LoadModel(string name)
		{
			foreach (Model model in models)
			{
				if (model.name == name)
					return model;
			}

			return null;
		}
	}
}
