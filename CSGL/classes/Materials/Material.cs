using System;
using System.Text.Json;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CSGL
{
	public class Material
	{
		public string Name;
		public ShaderProgram Shader;
		public Texture2D[] Textures;

		public int m_model;
		private int m_view;
		private int m_projection;

		public Matrix4 model_Matrix;

		public Material(ShaderProgram shader, Texture2D[] texture, string name) 
		{
			this.Shader = shader;
			this.Textures = texture;

			this.Name = name;

			m_model = GL.GetUniformLocation(shader.ShaderProgramHandle, "model");
			m_view = GL.GetUniformLocation(shader.ShaderProgramHandle, "view");
			m_projection = GL.GetUniformLocation(shader.ShaderProgramHandle, "projection");
		}
		
		public void MVP(Matrix4 model, Matrix4 view, Matrix4 projection)
		{
			GL.UseProgram(Shader.ShaderProgramHandle);

			GL.UniformMatrix4(m_model, true, ref model);
			GL.UniformMatrix4(m_view, true, ref view);
			GL.UniformMatrix4(m_projection, true, ref projection);

			GL.UseProgram(0);
		}

		public void Render()
		{
			if (Textures.Length > 0)
			{
				for (int i = 0; i < Textures.Length; i++)
				{
					this.Textures[i].UseTexture(TextureUnit.Texture0 + i);
				}
			}
		}

		public static Material LoadFromJson(string filePath)
		{
			string jsonString = File.ReadAllText(filePath);

			using (JsonDocument document = JsonDocument.Parse(jsonString))
			{
				JsonElement root = document.RootElement;

				ShaderProgram shader = ShaderManager.GetShader(root.GetProperty("shader").GetString() ?? "default");

				List<Texture2D> textures = new List<Texture2D>();

				if (root.TryGetProperty("textures", out JsonElement texturesElement))
				{
					foreach (JsonElement textureElement in texturesElement.EnumerateArray())
					{
						string texturePath = textureElement.GetString() ?? "default";

						Texture2D texture = TextureManager.GetTexture(texturePath);
						if (texture != null)
							textures.Add(texture);
					}
				}

				if (textures.Count > 32)
				{
					throw new Exception("Number of textures exceeds max");
				}

				if (shader != null)
				{
					string fileName = Path.GetFileNameWithoutExtension(filePath);
					Material mat = new Material(shader, textures.ToArray(), fileName);
					return mat;
				}
			}

			return MaterialManager.GetMaterial("default");
		}

	}
}
