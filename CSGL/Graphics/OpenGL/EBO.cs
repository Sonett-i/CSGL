using System;
using OpenTK.Graphics.OpenGL;
using Logging;

namespace CSGL.Graphics
{
	// EBO = Element Buffer Object
	public class EBO : IDisposable
	{
		public int ID;
		private bool initialized;
		public int indexLength;
		public uint[] buffer;

		public EBO(uint[] indices, BufferUsageHint hint = BufferUsageHint.StaticDraw)
		{
			this.ID = GL.GenBuffer();
			this.indexLength = indices.Length;
			this.buffer = indices;

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ID);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, hint);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			Log.GL($"Generated EBO: {this.ID}");
			this.initialized = true;
		}

		public void Bind()
		{
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ID);
		}

		public void Unbind()
		{
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
		}

		public void Dispose()
		{
			if (!initialized)
				return;

			GL.DeleteBuffer(this.ID);
			GC.SuppressFinalize(this);
		}
	}
}
