using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
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

			//this.vertexBuffer = new VertexBuffer(in MeshFilter, this.BufferUsageHint);
			//this.elementBuffers = new ElementBuffer[MeshFilter.Mesh.SubMesh.Length];
		}

		void Bind()
		{

		}

		public void Render()
		{
			
		}

		~MeshRenderer()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			
		}
	}
}
