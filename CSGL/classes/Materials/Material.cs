using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace CSGL
{
	public class Material
	{
		public string Name;
		public ShaderProgram Shader;
		public Texture2D Texture;

		public int m_model;
		private int m_view;
		private int m_projection;

		public Material(ShaderProgram shader, Texture2D texture, string name) 
		{
			this.Shader = shader;
			this.Texture = texture;

			this.Name = name;

			m_model = GL.GetUniformLocation(shader.ShaderProgramHandle, "model");
			m_view = GL.GetUniformLocation(shader.ShaderProgramHandle, "view");
			m_projection = GL.GetUniformLocation(shader.ShaderProgramHandle, "projection");
		}
		
		public void MVP(Matrix4 model, Matrix4 view, Matrix4 projection)
		{
			GL.UseProgram(Shader.ShaderProgramHandle);

			GL.UniformMatrix4(m_model, false, ref model);
			GL.UniformMatrix4(m_view, false, ref view);
			GL.UniformMatrix4(m_projection, false, ref projection);

			GL.UseProgram(0);
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
