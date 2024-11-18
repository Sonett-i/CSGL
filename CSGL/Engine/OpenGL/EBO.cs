using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL.Engine.OpenGL
{
	internal class EBO : IDisposable
	{
		int Handle;

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
