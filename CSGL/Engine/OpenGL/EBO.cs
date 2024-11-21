using System;
using OpenTK.Graphics.OpenGL;
using Logging;

namespace CSGL.Engine.OpenGL
{
	public class EBO : IDisposable
	{
		public uint[] Indices;
		public int ID;
		private bool initialized;

		public EBO(uint[] indices, BufferUsageHint hint = BufferUsageHint.StaticDraw)
		{
			this.Indices = indices;

			this.ID = GL.GenBuffer();

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ID);
			GL.BufferData(BufferTarget.ElementArrayBuffer, this.Indices.Length * sizeof(uint), this.Indices, hint);
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
