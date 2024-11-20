using System;
using ContentPipeline.Components;
using OpenTK.Graphics.OpenGL;
using Logging;

namespace CSGL.Engine.OpenGL
{
	internal class VBO : GLBuffer
	{
		public readonly float[] buffer;
		public readonly uint[] indices;
		
		private BufferUsageHint usageHint;

		public VBO(Vertex[] vertices, BufferUsageHint hint = BufferUsageHint.StaticDraw)
		{
			//this.buffer = mesh.ToVertexBuffer();

			base.ID = GL.GenBuffer();
			this.usageHint = hint;


			GL.BindBuffer(BufferTarget.ArrayBuffer, this.ID);
		}

		public override void Dispose()
		{
			base.Dispose();
		}
	}
}
