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


		private int buffer;

		private int vbo; // Vertex Buffer Object
		private int vao; // Vertex Array Object

		ShaderProgram shaderProgram;

		public RenderObject(float[] vertices, uint[] indices, ShaderProgram shaderProgram, BufferUsageHint hint = BufferUsageHint.StaticDraw)
		{

			this.vbo = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbo);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, hint);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

			this.vao = GL.GenBuffer();
			GL.BindVertexArray(this.vao);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			this.shaderProgram = shaderProgram;
			
			initialized = true;
		}



		public void Render()
		{
			GL.UseProgram(shaderProgram.ShaderProgramHandle);
			GL.BindVertexArray(vao);
			GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
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
				GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
				GL.DeleteBuffer(vbo);
				this.initialized = false;
			}

			this.disposed = true;
			GC.SuppressFinalize(this);
		}
	}
}
