using System;
using ContentPipeline;
using OpenTK.Graphics.OpenGL;
using Logging;
using SharedLibrary;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace CSGL.Graphics
{
	// VBO = Vertex Buffer Object
	public class VBO : IDisposable
	{
		public int ID;
		public readonly float[] buffer = null!;
		
		private BufferUsageHint usageHint;

		public VBO(Vertex[] vertices, BufferUsageHint hint = BufferUsageHint.StaticDraw)
		{
			this.usageHint = hint;

			this.buffer = Vertex.ToBuffer(vertices); // MeshData.Buffer(vertices);

			this.ID = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.ID);
			GL.BufferData(BufferTarget.ArrayBuffer, this.buffer.Length * sizeof(float), this.buffer, hint);

			Log.GL($"Generated VBO: {this.ID}");
		}

		public VBO(List<Matrix4> transforms, BufferUsageHint hint = BufferUsageHint.StaticDraw)
		{
			this.usageHint = hint;

			this.ID = GL.GenBuffer();

			int vec4size = Vector4.SizeInBytes;
			int mat4Size = 4 * vec4size;

			GL.BindBuffer(BufferTarget.ArrayBuffer, this.ID);
			GL.BufferData(BufferTarget.ArrayBuffer, transforms.Count * (mat4Size), transforms.ToArray(), hint);

			GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out int bufsize);

			this.buffer = new float[bufsize / sizeof(float)];

			GL.GetBufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, bufsize, this.buffer);

			Log.GL($"Generated IBO: {this.ID}");
		}

		public void Bind()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.ID);
		}

		public void Unbind()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}

		public void Dispose()
		{
			GL.DeleteBuffer(this.ID);
		}
	}
}
