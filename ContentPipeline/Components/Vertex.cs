using System;
using OpenTK.Mathematics;

namespace ContentPipeline.Components
{
	public struct Vertex
	{
		public readonly Vector3 position;
		public readonly Vector3 normal;
		public readonly Vector3 tangent;

		public readonly Vector2 UVs;

		public Vertex(Vector3 position, Vector3 normals, Vector3 tangent, Vector2 uvs)
		{
			this.position = position;
			this.normal = normals;
			this.tangent = tangent;

			this.UVs = uvs;
		}
	}
}
