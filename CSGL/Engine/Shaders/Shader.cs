using System;
using OpenTK.Graphics.OpenGL;

namespace CSGL
{
	public class Shader : IDisposable
	{
		bool disposed = false;
		public string Name;

		public ShaderProgram ShaderProgram;
		VertexShader VertexShader;
		FragmentShader FragmentShader;

		public Dictionary<string, ShaderUniform> Uniforms = new Dictionary<string, ShaderUniform>();
		public Dictionary<string, ShaderAttribute> Attributes = new Dictionary<string, ShaderAttribute>();


		public Shader(string Name, VertexShader vertexShader, FragmentShader fragmentShader)
		{
			this.Name = Name;
			this.VertexShader = vertexShader;
			this.FragmentShader = fragmentShader;

			this.ShaderProgram = new ShaderProgram(vertexShader, fragmentShader);

			GetUniforms();
			GetAttributes();

			Log.Default($"Compiled {Name} shader with {Uniforms.Count} uniforms, {Attributes.Count} attributes");
		}

		void GetUniforms()
		{
			ShaderUniform[] uniforms = this.ShaderProgram.CreateUniformList();

			foreach (ShaderUniform uniform in uniforms)
			{
				if (!Uniforms.ContainsKey(uniform.Name))
				{
					Uniforms[uniform.Name] = uniform;
				}
			}
		}

		void GetAttributes()
		{
			ShaderAttribute[] attributes = this.ShaderProgram.CreateAttributeList();

			foreach (ShaderAttribute attribute in attributes)
			{
				if (!Attributes.ContainsKey(attribute.Name))
				{
					Attributes[attribute.Name] = attribute;
				}
			}
		}

		~Shader() 
		{
			this.Dispose();
		}

		public void Dispose()
		{
			if (disposed)
				return;

			this.ShaderProgram.Dispose();
			this.VertexShader.Dispose();
			this.FragmentShader.Dispose();

			this.disposed = true;
			GC.SuppressFinalize(this);
		}
	}

	public class VertexShader : IDisposable
	{
		public string FileName;
		public string Code;
		public int VertexShaderHandle;

		public VertexShader(string fileName, string shaderCode)
		{
			this.FileName = fileName;
			this.Code = shaderCode;
			this.VertexShaderHandle = Compile(shaderCode);
		}

		int Compile(string shaderCode)
		{
			string errorMessage = string.Empty;

			int vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexShaderHandle, shaderCode);
			GL.CompileShader(vertexShaderHandle);

			string vertexShaderInfo = GL.GetShaderInfoLog(vertexShaderHandle);

			if (vertexShaderInfo != String.Empty)
			{
				errorMessage = vertexShaderInfo;
				return 0;
			}

			return vertexShaderHandle;
		}

		~VertexShader()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}

	public class FragmentShader : IDisposable
	{
		public string FileName;
		public string Code;
		public int FragmentShaderHandle;

		public FragmentShader(string fileName, string shaderCode)
		{
			this.FileName = fileName;
			this.Code = shaderCode;
			this.FragmentShaderHandle = Compile(shaderCode);
		}

		int Compile(string shaderCode)
		{
			string errorMessage = string.Empty;

			int fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShaderHandle, shaderCode);
			GL.CompileShader(fragmentShaderHandle);

			string fragmentShaderInfo = GL.GetShaderInfoLog(fragmentShaderHandle);

			if (fragmentShaderInfo != String.Empty)
			{
				errorMessage = fragmentShaderInfo;
				return 0;
			}

			return fragmentShaderHandle;
		}

		~FragmentShader()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
