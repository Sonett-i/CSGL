using System;

namespace ContentPipeline.Components
{
	public struct Mesh
	{
		public uint index = 0;
		public Vertex[] Vertices;
		public Material[] Materials;

		public Mesh(uint index, Vertex[] vertices, Material[] materials) 
		{
			this.index = index;
			this.Vertices = vertices;
			this.Materials = materials;
		}
	}
}
