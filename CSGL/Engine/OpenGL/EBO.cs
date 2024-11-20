using System;
using OpenTK.Graphics.OpenGL;

namespace CSGL.Engine.OpenGL
{
	internal class EBO : GLBuffer
	{

		public override void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
