using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CSGL
{
	public class Shader
	{
		int Handle;
		public int shaderProgramObject;
		private int vertexBufferObject;
		private int vertexArrayObject;

		public Shader(float[] vertices, string vertexPath, string fragmentPath)
		{
			this.vertexBufferObject = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBufferObject);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StreamDraw);

			this.vertexArrayObject = GL.GenVertexArray();
			GL.BindVertexArray(this.vertexArrayObject);
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBufferObject);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			Console.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");
			Console.WriteLine("Loading Vertex Shader: " + vertexPath + "\nLoading Fragment Shader: " + fragmentPath);
			
			string VertexShaderSource = @File.ReadAllText(vertexPath);
			string FragmentShaderSource = @File.ReadAllText(fragmentPath);


			int vertexShaderObject = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexShaderObject, @File.ReadAllText(vertexPath));
			GL.CompileShader(vertexShaderObject);

			string vertexShaderInfo = GL.GetShaderInfoLog(vertexShaderObject);

			if (vertexShaderInfo != String.Empty)
			{
				Console.WriteLine("Vertex Shader: " + vertexShaderInfo);
			}

			int fragmentShaderObject = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShaderObject, @File.ReadAllText(fragmentPath));
			GL.CompileShader(fragmentShaderObject);

			string fragmentShaderInfo = GL.GetShaderInfoLog(fragmentShaderObject);

			if (fragmentShaderInfo != String.Empty)
			{
				Console.WriteLine("Fragment Shader: " + fragmentShaderInfo);
			}

			this.shaderProgramObject = GL.CreateProgram();
			GL.AttachShader(shaderProgramObject, vertexShaderObject);
			GL.AttachShader(shaderProgramObject, fragmentShaderObject);
			GL.LinkProgram(shaderProgramObject);

			GL.DetachShader(this.shaderProgramObject, vertexShaderObject);
			GL.DetachShader(this.shaderProgramObject, fragmentShaderObject);

			GL.DeleteShader(vertexShaderObject);
			GL.DeleteShader(fragmentShaderObject);
		}

		private bool disposedValue = false;

		protected virtual void Dispose(bool disposing)
		{
            if (!disposedValue)
            {
				GL.DeleteProgram(Handle);
				disposedValue = true;
            }
        }
		~Shader()
		{
			if (disposedValue == false)
			{
				Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Use()
		{
			GL.UseProgram(this.shaderProgramObject);
		}
	}
}
/* Reference
 * 
 *	https://opentk.net/learn/chapter1/2-hello-triangle.html#compiling-the-shaders
 * 
 */