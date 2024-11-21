using System;
using OpenTK.Graphics.OpenGL;
using Logging;

namespace CSGL.Engine.OpenGL
{
	public abstract class GLBuffer : IDisposable
	{
		public int ID;
		private bool disposed = false;
		public bool initialized = false;

		~GLBuffer()
		{
			if (initialized)
				this.Dispose();
		}

		public virtual void Bind()
		{

		}

		public virtual void Unbind()
		{

		}

		public virtual void Dispose() 
		{
			if (disposed) return;

			GC.SuppressFinalize(this);
			disposed = true;
		}
	}
}
