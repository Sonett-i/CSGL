using System;
using System.Numerics;
using OpenTK.Graphics.OpenGL;

namespace CSGL
{
	public class RenderObject : IDisposable
	{
		private bool disposed;
		private bool initialized;

		private readonly int vertexArray;
		private readonly int buffer;
		private readonly int vertexCount;

		public RenderObject(Vertex[] vertices)
		{
			this.vertexCount = vertices.Length;
			this.vertexArray = GL.GenVertexArray();
			this.buffer = GL.GenBuffer();

			GL.BindVertexArray(this.vertexArray);
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexArray);

			GL.NamedBufferStorage(buffer, Vertex.Size * vertices.Length, vertices, BufferStorageFlags.MapWriteBit);

			GL.VertexArrayAttribBinding(vertexArray, 0, 0);
			GL.EnableVertexArrayAttrib(vertexArray, 0);
			GL.VertexArrayAttribFormat(vertexArray, 0, 4, VertexAttribType.Float, false, 0);

			GL.VertexArrayAttribBinding(vertexArray, 1, 0);
			GL.EnableVertexArrayAttrib(vertexArray, 1);
			GL.VertexArrayAttribFormat(vertexArray, 1, 4, VertexAttribType.Float, false, 16);

			GL.VertexArrayVertexBuffer(vertexArray, 0, buffer, IntPtr.Zero, Vertex.Size);
			this.initialized = true;
		}

		public void Render()
		{
			GL.BindVertexArray(vertexArray);
			GL.DrawArrays(PrimitiveType.Triangles, 0, this.vertexCount);
		}

		~RenderObject() 
		{
			this.Dispose();
		}
		public void Dispose() 
		{
			if (this.disposed)
				return;

			if (initialized)
			{
				GL.DeleteVertexArray(vertexArray);
				GL.DeleteBuffer(buffer);
				this.initialized = false;
			}

			this.disposed = true;
			GC.SuppressFinalize(this);
		}
	}
}
