using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CSGL.Graphics
{
	public class Framebuffer : IDisposable
	{
		public int ID;
		int fbTex;
		bool initialized = false;

		public Framebuffer()
		{
			
		}

		public void Dispose()
		{
			if (initialized)
				GL.DeleteFramebuffer(ID);

			GC.SuppressFinalize(this);
		}

	}
}
