using System;
using OpenTK.Graphics.OpenGL;
using Logging;

namespace CSGL.Engine.OpenGL
{
	internal class EBO : GLBuffer
	{
		public uint[] Indices;


		public EBO(uint[] indices)
		{
			this.Indices = indices;
		}

		public override void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
