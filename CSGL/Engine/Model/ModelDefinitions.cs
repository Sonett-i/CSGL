using System;
using OpenTK.Mathematics;

namespace CSGL
{
	public readonly struct Mesh
	{
		public readonly Vertex[] Vertices;
		public readonly uint[] Indices;

		public readonly string Name = "";
		public readonly int VertexCount;
		public readonly int Faces;

		public Mesh(Vertex[] vertices, uint[] indices, string name, int vertexCount, int faces)
		{
			this.Vertices = vertices;
			this.Indices = indices;
			this.Name = name;

			this.VertexCount = vertexCount;
			this.Faces = faces;
		}
	}

	public struct Vertex
	{
		public Vector3 Position;
		public Vector3 Normal;
		public Vector2 UV;

		public Vertex(Vector3 position, Vector3 normal, Vector2 UV)
		{
			this.Position = position;
			this.Normal = normal;
			this.UV = UV;
		}
	}
}