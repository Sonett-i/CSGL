using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

#pragma warning disable 8603

namespace CSGL
{
	public class ShaderManager
	{
		public static List<ShaderProgram> shaderList = new List<ShaderProgram>();

		public static void Initialize()
		{
			string[] files = Directory.GetFiles(EditorConfig.ShaderDirectory);
			Log.Default($"Compiling {files.Length} shaders in {EditorConfig.ShaderDirectory}");

			List<VertexShader> vertexShaders = new List<VertexShader>();
			List<FragmentShader> fragmentShaders = new List<FragmentShader>();

			for (int i = 0; i < files.Length; i++)
			{
				string ext = Path.GetExtension(files[i]);
				string fileName = Path.GetFileName(files[i]).Replace(ext, "");
				Shader shader;

				if (ext == ".vert")
				{
					//VertexShader shader = new VertexShader();
					shader = new VertexShader(fileName, File.ReadAllText(files[i]));

					vertexShaders.Add((VertexShader)shader);
				}

				if (ext == ".frag")
				{
					shader = new FragmentShader(fileName, File.ReadAllText(files[i]));
					fragmentShaders.Add((FragmentShader)shader);
				}

				Log.Advanced($"{fileName}{ext}");
			}

			foreach (VertexShader vertexShader in vertexShaders)
			{
				foreach (FragmentShader fragmentShader in fragmentShaders)
				{
					if (vertexShader.Name == fragmentShader.Name)
					{
						ShaderProgram shader = new ShaderProgram(vertexShader, fragmentShader, vertexShader.Name);
						shaderList.Add(shader);
					}
				}
			}

			Log.Default("Compiled Shaders\n");
		}

		public static void HotReload()
		{
			Log.Default("Reloading shaders");
			foreach (ShaderProgram shader in shaderList)
			{
				shader.Dispose();
			}
			
			ShaderManager.Initialize();
		}

		public static ShaderProgram GetShader(string name)
		{
			foreach (ShaderProgram shaderProgram in shaderList)
			{
				if (shaderProgram.Name == name)
				{
					return shaderProgram;
				}
			}

			return null;
		}

	}
}
