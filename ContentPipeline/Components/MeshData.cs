using System;
using OpenTK.Mathematics;


namespace ContentPipeline.Components
{
	public class MeshData
	{
		public readonly uint index = 0; // submesh index
		public Vertex[] Vertices { get; private set; }

		public uint[] Indices { get; private set; }
		public int VertexCount => Vertices.Length / Vertex.Stride;
		public int IndexCount => Indices.Length;
		public int Faces {  get; private set; }

		public MeshData(uint index, Vertex[] vertices, uint[] indices, int faceCount) 
		{
			this.index = index;
			this.Vertices = vertices;
			this.Faces = faceCount;
			this.Indices = indices;
		}

		public float Size
		{ 
			get 
			{
				return (Vertices.Length * (sizeof(float) * 11)) + Indices.Length * (sizeof(uint));
			} 
		}

		public override string ToString()
		{
			return $"Submesh Index ({index}): Vertices: {Vertices.Length}, Indices: {this.Indices.Length}, Faces: {this.Faces}";
		}

		/* 0	posX
		 * 1	posY
		 * 2	posZ
		 * 3	nX
		 * 4	nY
		 * 5	nZ
		 * 6	tX
		 * 7	tY
		 * 8	tZ
		 * 9	u
		 * 10	v
		 */

		public static float[] Buffer(Vertex[] vertices)
		{
			float[] buf = new float[vertices.Length * Vertex.Stride];

			int vIndex = 0;

			for (int i = 0; i < vertices.Length; i++)
			{
				vIndex = i * Vertex.Stride;

				// Position
				buf[vIndex] = vertices[i].position.X;
				buf[vIndex + 1] = vertices[i].position.Y;
				buf[vIndex + 2] = vertices[i].position.Z;

				// Normal
				buf[vIndex + 3] = vertices[i].normal.X;
				buf[vIndex + 4] = vertices[i].normal.Y;
				buf[vIndex + 5] = vertices[i].normal.Z;

				// Tangent
				buf[vIndex + 6] = vertices[i].tangent.X;
				buf[vIndex + 7] = vertices[i].tangent.Y;
				buf[vIndex + 8] = vertices[i].tangent.Z;

				// UV
				buf[vIndex + 9] = vertices[i].UV.X;
				buf[vIndex + 10] = vertices[i].UV.Y;
			}

			return buf;
		}
	}
}
