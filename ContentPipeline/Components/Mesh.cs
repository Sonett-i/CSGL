using System;
using OpenTK.Mathematics;

namespace ContentPipeline.Components
{
	public struct Mesh
	{
		public readonly uint index = 0; // submesh index
		public readonly Vertex[] Vertices = null!;
		public readonly uint[] Indices = null!;
		public readonly int FacesCount;

		public Mesh(uint index, Vertex[] vertices, uint[] indices, int faceCount) 
		{
			this.index = index;
			this.Vertices = vertices;
			this.Indices = indices;
			this.FacesCount = faceCount;
		}

		public readonly float Size
		{ 
			get 
			{
				return (Vertices.Length * (sizeof(float) * 11)) + Indices.Length * (sizeof(uint));
			} 
		}

		public override string ToString()
		{
			return $"Submesh Index ({index}): Vertices: {Vertices.Length}, Indices: {this.Indices.Length}, Faces: {this.FacesCount}";
		}
	}
}
