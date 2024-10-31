using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace CSGL
{
	public class Material
	{
		public string Name;
		public ShaderProgram Shader;
		public Texture2D Texture;

		public Material(ShaderProgram shader, Texture2D texture, string name) 
		{
			this.Shader = shader;
			this.Texture = texture;

			this.Name = name;
		}
		
		public static Material LoadFromJson(string filePath)
		{
			string jsonString = File.ReadAllText(filePath);

			using (JsonDocument document = JsonDocument.Parse(jsonString))
			{
				JsonElement root = document.RootElement;

				ShaderProgram shader = ShaderManager.GetShader(root.GetProperty("shader").GetString() ?? "default");
				Texture2D texture = TextureManager.GetTexture(root.GetProperty("texture").GetString() ?? "default");

				if (shader != null)
				{
					string fileName = Path.GetFileNameWithoutExtension(filePath);
					Material mat = new Material(shader, texture, fileName);
					return mat;
				}
			}


			return MaterialManager.GetMaterial("default");
		}

	}
}
