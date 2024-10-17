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
		public static void Initialize()
		{
			string[] files = Directory.GetFiles(EditorConfig.ModelDirectory);

			Log.Default($"Compiling {files.Length} models in {EditorConfig.ModelDirectory}");

			for (int i = 0; i < files.Length; i++)
			{
				string ext = Path.GetExtension(files[i]);
				string fileName = Path.GetFileName(files[i]).Replace(ext, "");

                if (ext == ".obj")
                {
					Obj obj = new Obj(File.ReadAllLines(EditorConfig.ModelDirectory + fileName + ext));
                }
                
			}
		}
	}
}
