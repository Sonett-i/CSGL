using System;
using System.Numerics;
using OpenTK.Graphics.OpenGL;

// dev branch

namespace CSGL
{
	public class RenderObject : IDisposable
	{
		private bool disposed;
		private bool initialized;

		private int vertexArray;
		private int buffer;
		
		public RenderObject(Vertex[] vertices, uint[] indices, ShaderProgram shaderProgram, BufferUsageHint hint = BufferUsageHint.StaticDraw)
		{
			
		}



		public void Render()
		{
			
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
