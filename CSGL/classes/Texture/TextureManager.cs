using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using StbImageSharp;

namespace CSGL
{
	public class TextureManager
	{
		public static List<Texture2D> Textures = new List<Texture2D>();
		public static Texture2D? defaultTexture;
		public static void Import()
		{
			Log.Default("Importing Textures");
			string[] files = Directory.GetFiles(EditorConfig.TextureDirectory);

			Log.Default($"Found {files.Length} texture(s) in {EditorConfig.TextureDirectory}");

			for (int i = 0; i < files.Length; i++)
			{
				string ext = Path.GetExtension(files[i]);
				string fileName = Path.GetFileName(files[i]);

				if (ext == ".png" || ext == ".jpeg")
				{
					Texture2D texture = new Texture2D(files[i], fileName);

					Textures.Add(texture);

					if (texture.Name == "default.png")
						defaultTexture = texture;

					Log.Default($"Loaded {fileName + ext}: " + texture.ToString());
				}
			}
		}

		public static void Import(string filePath, string fileName)
		{
			Texture2D texture = new Texture2D(filePath, fileName);
			Textures.Add(texture);
			Log.Default($"Loaded {fileName}: " + texture.ToString());
		}

		public static Texture2D GetTexture(string name)
		{
			foreach (Texture2D texture in Textures)
			{
				if (texture.Name == name)
				{
					return texture;
				}
			}

			return defaultTexture;
		}
	}
}
