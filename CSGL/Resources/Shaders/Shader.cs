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
		}
	}
}
/* Reference
 * 
 *	https://opentk.net/learn/chapter1/2-hello-triangle.html#compiling-the-shaders
 * 
 */