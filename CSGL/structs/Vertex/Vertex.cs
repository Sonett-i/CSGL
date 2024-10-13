using OpenTK.Mathematics;
using System;

namespace CSGL
{
	public struct Vertex
	{
		public static readonly int Size = (sizeof(float) + sizeof(float)) * 4; // size in bytes

		private readonly Vector4 position;
		private readonly Color4 colour;

		public Vertex(Vector4 position, Color4 colour)
		{
			this.position = position;
			this.colour = colour;
		}
	}
}
