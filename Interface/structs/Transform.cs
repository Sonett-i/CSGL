using System;
using MathU;

namespace Interface.structs
{
	public class Transform
	{
		public Vector3 Position;
		public Quaternion Rotation;
		public Vector3 Scale;

		public Transform()
		{
			this.Position = Vector3.Zero;
			this.Rotation = Quaternion.Identity;
			this.Scale = Vector3.Zero;
		}
	}
}
