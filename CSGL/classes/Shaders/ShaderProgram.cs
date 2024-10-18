﻿using System;
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

		public string Name;

		public readonly int ShaderProgramHandle;
		public readonly int VertexShaderHandle;
		public readonly int FragmentShaderHandle;

		private readonly ShaderUniform[] uniforms;
		private readonly ShaderAttribute[] attributes;

		public ShaderProgram(string vertexShaderCode, string fragmentShaderCode, string name)
		{
			this.Name = name;
			this.disposed = false;

			Log.Default("Compiling Vertex Shader: " + Name);

			Log.Advanced(File.ReadAllText(@vertexShaderCode));
			if (!ShaderProgram.CompileVertexShader(File.ReadAllText(@vertexShaderCode), out this.VertexShaderHandle, out string vertexShaderCompileError))
			{
				throw new ArgumentException(vertexShaderCompileError);
			}

			Log.Default("Compiling Fragment Shader: " + fragmentShaderCode);
			Log.Advanced(File.ReadAllText(fragmentShaderCode));
			if (!ShaderProgram.CompileFragmentShader(File.ReadAllText(@fragmentShaderCode), out this.FragmentShaderHandle, out string fragmentShaderCompileError))
			{
				throw new ArgumentException(fragmentShaderCompileError);
			}

			this.ShaderProgramHandle = ShaderProgram.CreateLinkProgram(VertexShaderHandle, FragmentShaderHandle);

			this.uniforms = ShaderProgram.CreateUniformList(this.ShaderProgramHandle);
			this.attributes = ShaderProgram.CreateAttributeList(this.ShaderProgramHandle);

			Log.Default($"{this.Name} compiled using handle: {this.ShaderProgramHandle}");

			if (EditorConfig.advancedDebug)
			{
				Log.Advanced($"Uniforms:{this.uniforms.Length}\nAttributes:{this.attributes.Length}");
			}
		}

		public ShaderProgram(VertexShader vertexShader, FragmentShader fragmentShader, string name)
		{
			this.Name = name;
			this.disposed = false;

			Log.Default("Compiling Vertex Shader: " + vertexShader.Name);

			Log.Advanced(vertexShader.ShaderCode);
			if (!ShaderProgram.CompileVertexShader(vertexShader.ShaderCode, out this.VertexShaderHandle, out string vertexShaderCompileError))
			{
				throw new ArgumentException(vertexShaderCompileError);
			}

			Log.Default("Compiling Fragment Shader: " + fragmentShader.Name);
			Log.Advanced(fragmentShader.ShaderCode);
			if (!ShaderProgram.CompileFragmentShader(fragmentShader.ShaderCode, out this.FragmentShaderHandle, out string fragmentShaderCompileError))
			{
				throw new ArgumentException(fragmentShaderCompileError);
			}

			this.ShaderProgramHandle = ShaderProgram.CreateLinkProgram(VertexShaderHandle, FragmentShaderHandle);

			this.uniforms = ShaderProgram.CreateUniformList(this.ShaderProgramHandle);
			this.attributes = ShaderProgram.CreateAttributeList(this.ShaderProgramHandle);

			Log.Default($"{this.Name} compiled using handle: {this.ShaderProgramHandle}");

			if (EditorConfig.advancedDebug)
			{
				Log.Advanced($"Uniforms:{this.uniforms.Length}\nAttributes:{this.attributes.Length}");
			}
		}

		~ShaderProgram()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			if (this.disposed)
				return;

			GL.DeleteShader(VertexShaderHandle);
			GL.DeleteShader(FragmentShaderHandle);

			GL.UseProgram(0);
			GL.DeleteProgram(this.ShaderProgramHandle);

			this.disposed = true;
			GC.SuppressFinalize(this);
		}

		public void Use()
		{
			GL.UseProgram(this.ShaderProgramHandle);
		}

		public static bool CompileVertexShader(string vertexShaderCode, out int vertexShaderHandle, out string errorMessage)
		{
			errorMessage = string.Empty;

			vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexShaderHandle, vertexShaderCode);
			GL.CompileShader(vertexShaderHandle);

			string vertexShaderInfo = GL.GetShaderInfoLog(vertexShaderHandle);

			if (vertexShaderInfo != String.Empty)
			{
				errorMessage = vertexShaderInfo;
				return false;
			}

			return true;
		}

		public static bool CompileFragmentShader(string fragmentShaderCode, out int fragmentShaderHandle, out string errorMessage)
		{
			errorMessage = string.Empty;

			fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShaderHandle, fragmentShaderCode);
			GL.CompileShader(fragmentShaderHandle);

			string fragmentShaderInfo = GL.GetShaderInfoLog(fragmentShaderHandle);

			if (fragmentShaderInfo != String.Empty)
			{
				errorMessage = fragmentShaderInfo;
				return false;
			}
			return true;
		}

		public static int CreateLinkProgram(int vertexShaderHandle, int fragmentShaderHandle)
		{
			int shaderProgramHandle = GL.CreateProgram();

			GL.AttachShader(shaderProgramHandle, vertexShaderHandle);
			GL.AttachShader(shaderProgramHandle, fragmentShaderHandle);
			GL.LinkProgram(shaderProgramHandle);

			GL.DetachShader(shaderProgramHandle, vertexShaderHandle);
			GL.DetachShader(shaderProgramHandle, fragmentShaderHandle);

			return shaderProgramHandle;
		}

		public ShaderUniform[] GetUniformList()
		{
			ShaderUniform[] result = new ShaderUniform[this.uniforms.Length];
			Array.Copy(this.uniforms, result, this.uniforms.Length);

			return result;
		}

		public ShaderAttribute[] GetAttributeList()
		{
			ShaderAttribute[] result = new ShaderAttribute[this.attributes.Length];
			Array.Copy(this.attributes, result, this.attributes.Length);

			return result;
		}

		public void SetUniform(string name, float v1)
		{
			if (!this.GetShaderUniform(name, out ShaderUniform uniform))
			{
				throw new ArgumentException("Uniform not found");
			}

			if (uniform.Type != ActiveUniformType.Float)
			{
				throw new ArgumentException("Uniform type mismatch");
			}

			GL.UseProgram(this.ShaderProgramHandle);
			GL.Uniform1(uniform.Location, v1);

			GL.UseProgram(0);
		}

		public void SetUniform(string name, float v1, float v2)
		{
			if (!this.GetShaderUniform(name, out ShaderUniform uniform))
			{
				throw new ArgumentException("Uniform not found");
			}

			if (uniform.Type != ActiveUniformType.FloatVec2)
			{
				throw new ArgumentException("Uniform type mismatch");
			}

			GL.UseProgram(this.ShaderProgramHandle);
			GL.Uniform2(uniform.Location, v1, v2);
			GL.UseProgram(0);
		}

		public void SetUniform(string name, float v1, float v2, float v3)
		{
			if (!this.GetShaderUniform(name, out ShaderUniform uniform))
			{
				throw new ArgumentException("Uniform not found");
			}

			if (uniform.Type != ActiveUniformType.FloatVec3)
			{
				throw new ArgumentException("Uniform type mismatch");
			}

			GL.UseProgram(this.ShaderProgramHandle);
			GL.Uniform3(uniform.Location, v1, v2, v3);
			GL.UseProgram(0);
		}

		public void SetUniform(string name, float v1, float v2, float v3, float v4)
		{
			if (!this.GetShaderUniform(name, out ShaderUniform uniform))
			{
				throw new ArgumentException("Uniform not found");
			}

			if (uniform.Type != ActiveUniformType.FloatVec4)
			{
				throw new ArgumentException("Uniform type mismatch");
			}

			GL.UseProgram(this.ShaderProgramHandle);
			GL.Uniform4(uniform.Location, v1, v2, v3, v4);
			GL.UseProgram(0);
		}

		public void SetUniform(string name, Matrix4 m1, bool transpose = false)
		{
			if (!this.GetShaderUniform(name, out ShaderUniform uniform))
			{
				throw new ArgumentException("Uniform not found");
			}

			if (uniform.Type != ActiveUniformType.FloatMat4)
			{
				throw new ArgumentException("Uniform type mismatch");
			}

			GL.UseProgram(this.ShaderProgramHandle);
			GL.UniformMatrix4(uniform.Location, transpose, ref m1);
			GL.UseProgram(0);
		}

		private bool GetShaderUniform(string name, out ShaderUniform uniform)
		{
			uniform = new ShaderUniform();

			for (int i = 0; i < this.uniforms.Length; i++)
			{
				uniform = this.uniforms[i];

				if (uniform.Name == name)
				{
					return true;
				}
			}
			return false;
		}

		public static ShaderUniform[] CreateUniformList(int shaderProgramhandle)
		{
			GL.GetProgram(shaderProgramhandle, GetProgramParameterName.ActiveUniforms, out int uniformCount);

			ShaderUniform[] uniforms = new ShaderUniform[uniformCount];

			for (int i = 0; i < uniformCount; i++)
			{
				GL.GetActiveUniform(shaderProgramhandle, i, 256, out _, out _, out ActiveUniformType type, out string name);
				int location = GL.GetUniformLocation(shaderProgramhandle, name);
				uniforms[i] = new ShaderUniform(name, location, type);
			}

			return uniforms;
		}

		public void SetAttribute(string name, float v1)
		{
			if (!this.GetShaderAttribute(name, out ShaderAttribute attribute))
			{
				throw new ArgumentException("Attribute not found");
			}

			if (attribute.Type != ActiveAttribType.Float)
			{
				throw new ArgumentException("Attribute type mismatch");
			}

			GL.UseProgram(this.ShaderProgramHandle);
			GL.VertexAttrib1(attribute.Location, v1);

			GL.UseProgram(0);
		}

		public void SetAttribute(string name, float v1, float v2)
		{
			if (!this.GetShaderAttribute(name, out ShaderAttribute attribute))
			{
				throw new ArgumentException("Attribute not found");
			}

			if (attribute.Type != ActiveAttribType.FloatVec2)
			{
				throw new ArgumentException("Attribute type mismatch");
			}

			GL.UseProgram(this.ShaderProgramHandle);
			GL.VertexAttrib2(attribute.Location, v1, v2);

			GL.UseProgram(0);
		}

		public void SetAttribute(string name, float v1, float v2, float v3)
		{
			if (!this.GetShaderAttribute(name, out ShaderAttribute attribute))
			{
				throw new ArgumentException("Attribute not found");
			}

			if (attribute.Type != ActiveAttribType.FloatVec3)
			{
				throw new ArgumentException("Attribute type mismatch");
			}

			GL.UseProgram(this.ShaderProgramHandle);
			GL.VertexAttrib3(attribute.Location, v1, v2, v3);

			GL.UseProgram(0);
		}

		public void SetAttribute(string name, float v1, float v2, float v3, float v4)
		{
			if (!this.GetShaderAttribute(name, out ShaderAttribute attribute))
			{
				throw new ArgumentException("Attribute not found");
			}

			if (attribute.Type != ActiveAttribType.FloatVec4)
			{
				throw new ArgumentException("Attribute type mismatch");
			}

			GL.UseProgram(this.ShaderProgramHandle);
			GL.VertexAttrib4(attribute.Location, v1, v2, v3, v4);

			GL.UseProgram(0);
		}

		private bool GetShaderAttribute(string name, out ShaderAttribute attribute)
		{
			attribute = new ShaderAttribute();

			for (int i = 0; i < this.attributes.Length; i++)
			{
				attribute = this.attributes[i];

				if (attribute.Name == name)
				{
					return true;
				}
			}
			return false;
		}

		public static ShaderAttribute[] CreateAttributeList(int shaderProgramhandle)
		{
			GL.GetProgram(shaderProgramhandle, GetProgramParameterName.ActiveAttributes, out int attributeCount);

			ShaderAttribute[] attributes = new ShaderAttribute[attributeCount];

			for (int i = 0; i < attributeCount; i++)
			{
				GL.GetActiveAttrib(shaderProgramhandle, i, 256, out _, out _, out ActiveAttribType type, out string name);
				int location = GL.GetAttribLocation(shaderProgramhandle, name);
				attributes[i] = new ShaderAttribute(name, location, type);
			}

			return attributes;
		}
	}
}