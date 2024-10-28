using System;
using OpenTK.Mathematics;

namespace CSGL.Math
{
	public struct Rotor
	{
		public float x;
		public float y;
		public float z;
		public float w;

		public Rotor(float angle, Vector3 axis)
		{
			axis = Vector3.Normalize(axis);

			angle = MathHelper.DegreesToRadians(angle);

			float halfAngle = angle * 0.5f;
			float sHA = MathF.Sin(halfAngle);

			this.w = MathF.Cos(halfAngle);
			this.x = axis.X * sHA;
			this.y = axis.Y * sHA;
			this.z = axis.Z * sHA;
		}

		public Rotor(float w, float x, float y, float z)
		{
			this.w = w;
			this.x = x;
			this.y = y;
			this.z = z;

			Normalize();
		}

		public void Normalize()
		{
			float mag = MathF.Sqrt(w*w + x*x + y*y + z*z);

			w /= mag;
			x /= mag;
			y /= mag;
			z /= mag;
		}

		public static Rotor Multiply(Rotor left, Rotor right)
		{
			return new Rotor(
				left.w * right.w - left.x * right.x - left.y * right.y - left.z * right.z,
				left.w * right.w + left.x * right.w + left.y * right.z - left.z * right.y,
				left.w * right.y - left.x * right.z + left.y * right.w + left.z * right.x,
				left.w * right.z + left.x * right.y - left.y * right.x + left.z * right.w
				);
		}

		public Quaternion ToQuaternion()
		{
			Quaternion result = new Quaternion(x, y, z, w);

			return result;
		}
	}
}
