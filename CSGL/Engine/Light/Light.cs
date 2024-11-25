using System;
using OpenTK.Mathematics;

namespace CSGL.Engine
{
	public class Light : Entity
	{

		public Vector4 Albedo = Vector4.Zero;
		public float intensity = 1.0f;

		public Light() : base("Light")
		{

		}
	}
}
