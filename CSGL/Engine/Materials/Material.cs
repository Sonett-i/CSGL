using System;
using System.Text.Json;
using Assimp.Unmanaged;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CSGL
{
	public class Material
	{
		public static Material DefaultMaterial;
		public static Material BadMaterial;

		public string Name;
		public Shader Shader;

		//public ShaderProgram Shader;
		public Texture2D[] Textures;

		public int m_model;
		private int m_view;
		private int m_projection;

		public Matrix4 model_Matrix;

		public Material(Shader shader, Texture2D[] texture, string name) 
		{
			this.Shader = shader;
			this.Textures = texture;

			this.Name = name;

			m_model = GL.GetUniformLocation(Shader.ShaderProgram.ShaderProgramHandle, "model");
			m_view = GL.GetUniformLocation(Shader.ShaderProgram.ShaderProgramHandle, "view");
			m_projection = GL.GetUniformLocation(Shader.ShaderProgram.ShaderProgramHandle, "projection");
		}
		
		public void MVP(Matrix4 model, Matrix4 view, Matrix4 projection)
		{
			GL.UseProgram(Shader.ShaderProgram.ShaderProgramHandle);

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

			string materialName = "";
			string shaderFile = "";
			List<string> textures = new List<string>();

			using (JsonDocument document = JsonDocument.Parse(jsonString))
			{
				JsonElement root = document.RootElement;

				materialName = root.GetProperty("name").GetString() ?? "default";
				shaderFile = root.GetProperty("shader").GetString() ?? "default";

				if (root.TryGetProperty("textures", out JsonElement texturesElement))
				{
					foreach (JsonElement textureElement in texturesElement.EnumerateArray())
					{
						textures.Add(textureElement.GetString() ?? "default");	
					}
				}

				if (textures.Count > 32)
				{
					return Material.DefaultMaterial;
				}
			}

			Shader shader = Resources.Shaders[shaderFile];

			Texture2D[] materialTextures = new Texture2D[textures.Count];

			for (int i = 0; i < textures.Count; i++)
			{
				materialTextures[i] = Resources.Textures[textures[i]];
			}

			Material material = new Material(shader, materialTextures, materialName) ?? Material.DefaultMaterial;

			return material;
		}

	}
}
