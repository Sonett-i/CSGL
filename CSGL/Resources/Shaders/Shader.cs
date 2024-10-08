using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CSGL
{
	public class Shader
	{
		int Handle = 0;
		public int shaderProgramObject;
		private int vertexBufferObject;
		private int vertexArrayObject;
		private int elementBufferObject;

		void DebugShader(float[] vertices, uint[] indices)
		{
			string output = "Vertices\n";

			for (int i = 0; i < vertices.Length; i++)
			{
				output += vertices[i] + ", ";

				if (i % 7 == 0)
					output += "\n";
			}

			output += "\nIndices:\n";
			for (int i = 0; i < indices.Length; i++)
			{
				output += indices[i] + ", ";

				if (i % 3 == 0)
					output += "\n";
			}

			Console.WriteLine(output);
		}

		public Shader(float[] vertices, uint[] indices, string vertexPath, string fragmentPath, int handle = 0)
		{
			DebugShader(vertices, indices);

			this.Handle = handle;

			this.vertexBufferObject = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBufferObject);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StreamDraw);

			this.vertexArrayObject = GL.GenVertexArray();
			GL.BindVertexArray(this.vertexArrayObject);
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBufferObject);

			// Positional Data
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 7 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			// Vertex Colour
			GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 7 * sizeof(float), 3 * sizeof(float));
			GL.EnableVertexAttribArray(1);

			Console.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");
			Console.WriteLine("Loading Vertex Shader: " + vertexPath);
			Console.WriteLine(File.ReadAllText(vertexPath));

			Console.WriteLine("\nLoading Fragment Shader: " + fragmentPath);
			Console.WriteLine(File.ReadAllText(fragmentPath));



			// Indices
			this.elementBufferObject = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.elementBufferObject);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

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

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			GL.DeleteBuffer(this.elementBufferObject);

			GL.BindVertexArray(0);
			GL.DeleteVertexArray(this.vertexArrayObject);

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.DeleteBuffer(this.vertexBufferObject);

			GL.UseProgram(0);
			GL.DeleteProgram(this.shaderProgramObject);

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