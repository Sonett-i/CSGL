using System;
using OpenTK.Mathematics;

namespace CSGL
{
	public class Transform
	{
		public Vector3 Position;
		public Quaternion Rotation; 
		public Vector3 Scale;

		public Transform(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			Position = position;
			Rotation = rotation;
			Scale = scale;
		}
	}
}
