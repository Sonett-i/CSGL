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
		private int ebo; // Element Buffer Object

		float[] vertices;
		uint[] indices;

		ShaderProgram shaderProgram;
		BufferUsageHint hint;

		public RenderObject(float[] vertices, uint[] indices, ShaderProgram shaderProgram, BufferUsageHint hint = BufferUsageHint.StaticDraw)
		{
			this.vertices = vertices;
			this.indices = indices;
			this.hint = hint;

			Console.WriteLine("Bind Vertex Buffer Object");
			this.vbo = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbo);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, hint);

			Console.WriteLine("Bind Vertex Array Object");
			this.vao = GL.GenVertexArray();
			GL.BindVertexArray(this.vao);
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbo);

			// Positional Data
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 7 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			// Vertex Colour
			GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 7 * sizeof(float), 3 * sizeof(float));
			GL.EnableVertexAttribArray(1);

			// Indices
			Console.WriteLine("Bind Index Buffer Object");
			this.ebo = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ebo);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);


			GL.BindVertexArray(0);
			this.shaderProgram = shaderProgram;
			
			initialized = true;
		}

		public void Render()
		{
			GL.UseProgram(shaderProgram.ShaderProgramHandle);

			GL.BindVertexArray(this.vao);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ebo);

			GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
			//GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

			ErrorCode error = GL.GetError();

			if (error != ErrorCode.NoError)
			{
				Console.WriteLine($"OpenGL Error: {error}");
			}

			//
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
				GL.DeleteBuffer(vbo);
				GL.DeleteBuffer(ebo);
				GL.DeleteVertexArray(vao);

				this.initialized = false;
			}

			this.disposed = true;
			GC.SuppressFinalize(this);
		}
	}
}
