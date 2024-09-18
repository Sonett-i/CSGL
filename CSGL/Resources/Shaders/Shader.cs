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

		public Shader(string vertexPath, string fragmentPath)
		{
			int VertexShader;
			int FragmentShader;

			Console.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");
			Console.WriteLine("Loading Vertex Shader: " + vertexPath + "\nLoading Fragment Shader: " + fragmentPath);
			
			string VertexShaderSource = File.ReadAllText(vertexPath);
			string FragmentShaderSource = File.ReadAllText(fragmentPath);

			VertexShader = GL.CreateShader(ShaderType.VertexShader);
			FragmentShader = GL.CreateShader(ShaderType.FragmentShader);

			GL.CompileShader(VertexShader);
			GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int vertSuccess);

			if (vertSuccess == 0)
			{
				string infoLog = GL.GetShaderInfoLog(VertexShader);
				Console.WriteLine(infoLog);
			}

			GL.CompileShader(FragmentShader);
			GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out int fragSuccess);

			if (fragSuccess == 0)
			{
				string infoLog = GL.GetShaderInfoLog(FragmentShader);
				Console.WriteLine(infoLog);
			}

			Handle = GL.CreateProgram();

			GL.AttachShader(Handle, VertexShader);
			GL.AttachShader(Handle, FragmentShader);

			GL.LinkProgram(Handle);

			GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int linkSuccess);

			if (linkSuccess == 0)
			{
				string infoLog = GL.GetProgramInfoLog(Handle);
				Console.WriteLine(infoLog);
			}

			// Cleanup
			GL.DetachShader(Handle, VertexShader);
			GL.DetachShader(Handle, FragmentShader);
			GL.DeleteShader(VertexShader);
			GL.DeleteShader(FragmentShader);
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
			GL.UseProgram(Handle);
		}
	}
}
/* Reference
 * 
 *	https://opentk.net/learn/chapter1/2-hello-triangle.html#compiling-the-shaders
 * 
 */