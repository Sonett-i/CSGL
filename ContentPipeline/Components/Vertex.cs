using System;

namespace ContentPipeline.Components
{
	public struct Vertex
	{
		public readonly float X;
		public readonly float Y;
		public readonly float Z;

		public readonly float normalX;
		public readonly float normalY;
		public readonly float normalZ;

		public readonly float U, V;

		public Vertex(float x, float y, float z, float normalX, float normalY, float normalZ, float u, float v)
		{
			X = x;
			Y = y;
			Z = z;
			this.normalX = normalX;
			this.normalY = normalY;
			this.normalZ = normalZ;
			U = u;
			V = v;
		}
	}
}
