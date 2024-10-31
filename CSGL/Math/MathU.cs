using System;
using OpenTK.Mathematics;

namespace CSGL
{
	public static class MathU
	{
		public static Quaternion Euler(Vector3 eulerAngles)
		{
			return Euler(eulerAngles.X, eulerAngles.Y, eulerAngles.Z);
		}

		public static Quaternion Euler(float x, float y, float z)
		{
			x = (MathHelper.DegreesToRadians(x) /2);
			y = (MathHelper.DegreesToRadians(y) / 2);
			z = (MathHelper.DegreesToRadians(z) / 2);

			float c1 = MathF.Cos(x);
			float s1 = MathF.Sin(x);

			float c2 = MathF.Cos(y);
			float s2 = MathF.Sin(y);

			float c3 = MathF.Cos(z);
			float s3 = MathF.Sin(z);

			Quaternion result = new Quaternion(
				c1 * c2 * c3 + s1 * s2 * s3, // x
				c1 * c2 * s3 - s1 * s2 * c3, // y
				c1 * s2 * c3 + s1 * c2 * s3, // z
				s1 * c2 * c3 - c1 * s2 * s3); // w

			return result;
		}

		public static float Clamp(float x, float min, float max)
		{
			float t = (x < min) ? min : x;
			return (t > max) ? max : t;
		}

		public static Matrix4 TRS(Transform transform)
		{
			Matrix4 translation = Matrix4.CreateTranslation(transform.Position);
			Matrix4 rotation = Matrix4.CreateFromQuaternion(transform.Rotation);
			Matrix4 scale = Matrix4.CreateScale(transform.Scale);

			return scale * rotation * translation;
		}

		public static float Rad(float degrees)
		{
			return MathF.PI * degrees / 180;
		}
	}
}

/*
 * Baker, M. J. (2023). Euler to Quaternion. Retrieved from EuclideanSpace - Mathematics and Computing: http://www.euclideanspace.com/maths/geometry/rotations/conversions/eulerToQuaternion/index.htm
 * Brett, M. (2016). Rotations and rotation matrices. Retrieved from Psych 214 – functional MRI methods: https://bic-berkeley.github.io/psych-214-fall-2016/rotation_2d_3d.html
 * 
 */