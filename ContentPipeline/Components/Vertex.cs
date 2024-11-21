using System;
using OpenTK.Mathematics;

namespace ContentPipeline.Components
{
	public struct Vertex
	{
		public readonly Vector3 position;
		public readonly Vector3 normal;
		public readonly Vector3 tangent;

		public readonly Vector2 UV;

		public Vertex(Vector3 position, Vector3 normals, Vector3 tangent, Vector2 uvs)
		{
			this.position = position;
			this.normal = normals;
			this.tangent = tangent;

			this.UV = uvs;
		}

		public static int Stride = 12;

		public static int PositionOffset = 0;
		public static int NormalOffset = PositionOffset + 3;
		public static int TangentOffset = PositionOffset + NormalOffset + 3;
		public static int UVOffset = PositionOffset + NormalOffset + TangentOffset;
	}
}