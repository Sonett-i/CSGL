using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CSGL
{
	public static class ModelManager
	{
		static List<Model> models = new List<Model>();

		public static void Initialize()
		{
			string[] files = Directory.GetFiles(EditorConfig.ModelDirectory);

			Log.Default($"Found {files.Length} model(s) in {EditorConfig.ModelDirectory}");

			for (int i = 0; i < files.Length; i++)
			{
				string ext = Path.GetExtension(files[i]);
				string fileName = Path.GetFileName(files[i]).Replace(ext, "");

                if (ext == ".obj")
                {
					Model model = Obj.Import(File.ReadAllLines(EditorConfig.ModelDirectory + fileName + ext));
					models.Add(model);
					Log.Default($"Loaded {fileName + ext}: " + model.ToString());
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
