using System;
using OpenTK.Graphics.OpenGL;

namespace CSGL.Engine
{
	public class Shader : IDisposable
	{
		public uint ID;
		public string Name;

		public Shader(string name, string vertexFile, string fragmentFile)
		{

		}

		public void Activate()
		{
			GL.UseProgram(ID);
		}

		public void Delete()
		{
			this.Dispose();
		}

		public void Compile()
		{

		}

		public void Dispose()
		{
			GL.DeleteProgram(ID);
			GC.SuppressFinalize(this);
		}
	}
}
