using OpenTK.Graphics.ES20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL
{
	public class MeshRenderer : IDisposable
	{
		public Material material;
		public MeshFilter MeshFilter;
		public BufferUsageHint BufferUsageHint;

		public MeshRenderer(MeshFilter MeshFilter, Material material, BufferUsageHint hint = BufferUsageHint.StaticDraw) 
		{
			this.MeshFilter = MeshFilter;
			this.material = material;
			this.BufferUsageHint = hint;
		}

		void Bind()
		{

		}

		public void Render()
		{

		}

		public void Dispose()
		{

		}
	}
}
