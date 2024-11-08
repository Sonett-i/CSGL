using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CSGL
{
	

	public readonly struct ShaderUniform
	{
		public readonly string Name;
		public readonly int Location;
		public readonly ActiveUniformType Type;

		public ShaderUniform(string name, int location, ActiveUniformType type)
		{
			this.Name = name;
			this.Location = location;
			this.Type = type;
		}
	}

	public readonly struct ShaderAttribute 
	{
		public readonly string Name;
		public readonly int Location;
		public readonly ActiveAttribType Type;

		public ShaderAttribute(string name, int location, ActiveAttribType type)
		{
			this.Name = name;
			this.Location = location;
			this.Type = type;
		}
	}

	public sealed class ShaderProgram : IDisposable
	{
		private bool disposed;

		public readonly int ShaderProgramHandle;


		public ShaderProgram(VertexShader vertexShader, FragmentShader fragmentShader)
		{
			this.disposed = false;

			this.ShaderProgramHandle = CreateLinkProgram(vertexShader.VertexShaderHandle, fragmentShader.FragmentShaderHandle);

			CheckCompileStatus(vertexShader.VertexShaderHandle);
			CheckCompileStatus(fragmentShader.FragmentShaderHandle);

			CheckProgramLinkStatus();
		}

		public void Use()
		{
			GL.UseProgram(this.ShaderProgramHandle);
		}

		public int CreateLinkProgram(int vertexShaderHandle, int fragmentShaderHandle)
		{
			int shaderProgramHandle = GL.CreateProgram();

			GL.AttachShader(shaderProgramHandle, vertexShaderHandle);
			GL.AttachShader(shaderProgramHandle, fragmentShaderHandle);
			GL.LinkProgram(shaderProgramHandle);

			GL.DetachShader(shaderProgramHandle, vertexShaderHandle);
			GL.DetachShader(shaderProgramHandle, fragmentShaderHandle);

			return shaderProgramHandle;
		}

		public void CheckCompileStatus(int handle)
		{
			GL.GetShader(handle, ShaderParameter.CompileStatus, out int status);

			if (status == 0)
			{
				string infoLog = GL.GetShaderInfoLog(this.ShaderProgramHandle);
				throw new InvalidOperationException($"Shader compile error {infoLog}");
			}
		}

		public void CheckProgramLinkStatus()
		{
			GL.GetProgram(this.ShaderProgramHandle, GetProgramParameterName.LinkStatus, out int status);

			if (status == 0)
			{
				string infoLog = GL.GetProgramInfoLog(this.ShaderProgramHandle);
				throw new InvalidOperationException($"Program link error: {infoLog}");
			}
		}

		public ShaderUniform[] CreateUniformList()
		{
			GL.GetProgram(this.ShaderProgramHandle, GetProgramParameterName.ActiveUniforms, out int uniformCount);

			ShaderUniform[] uniforms = new ShaderUniform[uniformCount];

			for (int i = 0; i < uniformCount; i++)
			{
				GL.GetActiveUniform(this.ShaderProgramHandle, i, 256, out _, out _, out ActiveUniformType type, out string name);
				int location = GL.GetUniformLocation(this.ShaderProgramHandle, name);
				uniforms[i] = new ShaderUniform(name, location, type);
			}

			return uniforms;
		}

		public ShaderAttribute[] CreateAttributeList()
		{
			GL.GetProgram(this.ShaderProgramHandle, GetProgramParameterName.ActiveAttributes, out int attributeCount);

			ShaderAttribute[] attributes = new ShaderAttribute[attributeCount];

			for (int i = 0; i < attributeCount; i++)
			{
				GL.GetActiveAttrib(this.ShaderProgramHandle, i, 256, out _, out _, out ActiveAttribType type, out string name);
				int location = GL.GetAttribLocation(this.ShaderProgramHandle, name);
				attributes[i] = new ShaderAttribute(name, location, type);
			}

			return attributes;
		}

		// Dispose shader resources when class destructor is called
		~ShaderProgram()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			if (this.disposed)
				return;

			GL.UseProgram(0);
			GL.DeleteProgram(this.ShaderProgramHandle);

			this.disposed = true;
			GC.SuppressFinalize(this);
		}
	}
}
