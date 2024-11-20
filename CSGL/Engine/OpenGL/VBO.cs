using System;
using ContentPipeline.Components;
using OpenTK.Graphics.OpenGL;

namespace CSGL.Engine.OpenGL
{
	internal class VBO : GLBuffer
	{
		public readonly float[] buffer;
		public readonly uint[] indices;
		
		private BufferUsageHint usageHint;

		public VBO(Mesh mesh, BufferUsageHint hint = BufferUsageHint.StaticDraw)
		{
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
