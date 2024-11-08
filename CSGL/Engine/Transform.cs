using System;
using OpenTK.Mathematics;

namespace CSGL
{
	public class Transform : Component
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

		public Transform()
		{
			this.Position = Vector3.Zero;
			this.Rotation = Quaternion.Identity;
			this.Scale = Vector3.One;
		}

		public Vector3 LocalRotation
		{
			get
			{
				return Rotation.ToEulerAngles();
			}
		}

		public Vector3 Forward
		{
			get
			{
				float radY = MathHelper.DegreesToRadians(LocalRotation.Y);
				float radX = MathHelper.DegreesToRadians(LocalRotation.X);

				return new Vector3(
					(float)(MathF.Cos(radY) * MathF.Cos(radX)),
					(float)(MathF.Sin(radX)),
					(float)(MathF.Sin(radY) * MathF.Cos(radX))
					);
			}
		}

		public Vector3 Right
		{
			get
			{
				return Vector3.Cross(Forward, Vector3.UnitY).Normalized();
			}
		}
	}
}
