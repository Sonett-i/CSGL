using System;
using OpenTK.Graphics.OpenGL;
using Logging;

namespace CSGL.Engine.OpenGL
{
	public abstract class GLBuffer : IDisposable
	{
		public int ID;
		private bool _disposed = false;
		private bool initialized = false;

		~GLBuffer()
		{
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
			if (_disposed) return;

			GC.SuppressFinalize(this);
		}
	}
}
