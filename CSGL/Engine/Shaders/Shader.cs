using System;
using Assimp;
using OpenTK.Graphics.OpenGL;
using Logging;

namespace CSGL.Engine
{
	public class Shader : IDisposable
	{
		private bool disposed;
		public int ID;
		public string Name;

		public Dictionary<string, int> Uniforms = new Dictionary<string, int>();
		public Dictionary<string, int> Attributes = new Dictionary<string, int>();

		private VertexShader vertexShader;
		private FragmentShader fragmentShader;

		public Shader(string name, string vertexFile, string fragmentFile)
		{
			this.disposed = false;

			this.Name = name;

			Log.GL($"Compiling {this.Name}");
			this.vertexShader = new VertexShader(vertexFile);
			this.fragmentShader = new FragmentShader(fragmentFile);

			if (CheckCompileStatus(vertexShader.ID) == 0)
				throw new Exception("Vertex Shader Failed to Compile");

			if (CheckCompileStatus(fragmentShader.ID) == 0)
				throw new Exception("Fragment Shader Failed to Compile");

			this.ID = this.Compile(this.vertexShader, this.fragmentShader);

			if (CheckProgramLinkStatus() == 0)
				throw new Exception("Fragment Shader Failed to Compile");

		}

		public void Activate()
		{
			GL.UseProgram(ID);
		}

		public void Delete()
		{
			this.Dispose();
		}

		int Compile(VertexShader vertexShader, FragmentShader fragmentShader)
		{
			int shaderProgramHandle = GL.CreateProgram();

			GL.AttachShader(shaderProgramHandle, vertexShader.ID);
			GL.AttachShader(shaderProgramHandle, fragmentShader.ID);
			GL.LinkProgram(shaderProgramHandle);

			GL.DetachShader(shaderProgramHandle, vertexShader.ID);
			GL.DetachShader(shaderProgramHandle, fragmentShader.ID);

			return shaderProgramHandle;
		}

		private int CheckCompileStatus(int handle)
		{
			GL.GetShader(handle, ShaderParameter.CompileStatus, out int status);

			if (status == 0)
			{
				string infoLog = GL.GetShaderInfoLog(handle);
				Log.GL($"Error: {infoLog}");
			}

			return status;
		}

		private int CheckProgramLinkStatus()
		{
			GL.GetProgram(this.ID, GetProgramParameterName.LinkStatus, out int status);

			if (status == 0)
			{
				string infoLog = GL.GetProgramInfoLog(this.ID);
				Log.GL($"Error: {infoLog}");
			}

			return status;
		}

		~Shader()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			if (!disposed)
			{
				vertexShader.Dispose();
				fragmentShader.Dispose();

				GL.DeleteProgram(this.ID);
				GC.SuppressFinalize(this);

				disposed = true;
			}
		}

		private class VertexShader : IDisposable
		{
			public int ID;
			private bool disposed = false;

			public VertexShader(string vertFile)
			{
				this.ID = Compile(vertFile);
			}

			int Compile(string vertFile)
			{
				string fileName = Path.GetFileName(vertFile);
				string shaderCode = File.ReadAllText(vertFile);

				Log.GL($"Compiling {fileName}: {vertFile}");

				int vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
				GL.ShaderSource(vertexShaderHandle, shaderCode);
				GL.CompileShader(vertexShaderHandle);

				string vertexShaderInfo = GL.GetShaderInfoLog(vertexShaderHandle);
				if (vertexShaderInfo != string.Empty)
				{
					Log.GL($"Error compiling vertex shader {shaderCode}");
					return 0;
				}

				Log.GL($"{fileName} successfully compiled using handle: {vertexShaderHandle}");
				return vertexShaderHandle;
			}

			~VertexShader()
			{
				this.Dispose();
			}

			public void Dispose()
			{
				if (disposed) return;

				GL.DeleteShader(this.ID);

				disposed = true;
				GC.SuppressFinalize(this);
			}
		}

		private class FragmentShader : IDisposable
		{
			public int ID;
			private bool disposed = false;

			public FragmentShader(string fragFile)
			{
				this.ID = Compile(fragFile);
			}

			int Compile(string fragFile)
			{
				string fileName = Path.GetFileName(fragFile);
				string shaderCode = File.ReadAllText(fragFile);

				Log.GL($"Compiling {fileName}: {fragFile}");

				int fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
				GL.ShaderSource(fragmentShaderHandle, shaderCode);
				GL.CompileShader(fragmentShaderHandle);

				string fragmentShaderInfo = GL.GetShaderInfoLog(fragmentShaderHandle);
				if (fragmentShaderInfo != string.Empty)
				{
					Log.GL($"Error compiling vertex shader {shaderCode}");
				}

				Log.GL($"{fileName} successfully compiled using handle: {fragmentShaderHandle}");
				return fragmentShaderHandle;
			}

			~FragmentShader()
			{
				this.Dispose();
			}

			public void Dispose()
			{
				if (disposed) return;

				GL.DeleteShader(this.ID);
				GC.SuppressFinalize(this);
				this.disposed = true;
			}
		}
	}
}
