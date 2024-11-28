using OpenTK.Mathematics;

namespace CSGL.Graphics
{
	public struct Vertex
	{
		public readonly Vector3 position;
		public readonly Vector3 normal;
		public readonly Vector3 tangent;
		public readonly Vector3 bitangent;

		public readonly Vector2 UV;

		public Vertex(Vector3 position, Vector3 normals, Vector3 tangent, Vector3 bitangent, Vector2 uvs)
		{
			this.position = position;
			this.normal = normals;
			this.tangent = tangent;
			this.bitangent = bitangent;

			this.UV = uvs;
		}

		public Vertex(float posX, float posY, float posZ, float nX, float nY, float nZ, float tX, float tY, float tZ, float btX, float btY, float btZ, float u, float v)
		{
			this.position = new Vector3(posX, posY, posZ);
			this.normal = new Vector3(nX, nY, nZ);
			this.tangent = new Vector3(tX, tY, tZ);
			this.bitangent = new Vector3(btX, btY, btZ);

			this.UV = new Vector2(u, v);
		}

		public static int Stride = 32;

		public static int PositionOffset = 0;
		public static int NormalOffset = PositionOffset + 3;
		public static int TangentOffset = PositionOffset + NormalOffset + 3;
		public static int UVOffset = PositionOffset + NormalOffset + TangentOffset;

		public static float[] ToBuffer(Vertex[] vertices)
		{
			float[] buffer = new float[vertices.Length * Vertex.Stride];

			int index = 0;

			for (int i = 0; i < vertices.Length; i++)
			{
				index = i * Stride;

				buffer[index] = vertices[i].position.X;
				buffer[index + 1] = vertices[i].position.Y;
				buffer[index + 2] = vertices[i].position.Z;

				buffer[index + 3] = vertices[i].normal.X;
				buffer[index + 4] = vertices[i].normal.Y;
				buffer[index + 5] = vertices[i].normal.Z;

				buffer[index + 6] = vertices[i].tangent.X;
				buffer[index + 7] = vertices[i].tangent.Y;
				buffer[index + 8] = vertices[i].tangent.Z;

				buffer[index + 9] = vertices[i].UV.X;
				buffer[index + 10] = vertices[i].UV.Y;
			}

			return buffer;
		}
	}
}
