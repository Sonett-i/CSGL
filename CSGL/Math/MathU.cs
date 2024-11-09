﻿using System;
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

		public static Matrix4 TRS(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			Matrix4 Translate = Matrix4.CreateTranslation(position);
			Matrix4 Rotate = Matrix4.CreateFromQuaternion(rotation);
			Matrix4 Scale = Matrix4.CreateScale(scale);

			return Scale * Rotate * Translate;
		}

		public static Quaternion LookRotation(Vector3 forward, Vector3 up)
		{
			forward = forward.Normalized();

			Vector3 right = Vector3.Cross(up, forward).Normalized();
			Vector3 cameraUp = Vector3.Cross(forward, right);

			Quaternion rotation = new Quaternion();
			rotation.W = MathF.Sqrt(1.0f + right.X + cameraUp.Y + forward.Z) * 0.5f;
			float w4 = (4.0f * rotation.W);
			rotation.X = (cameraUp.Z - forward.Z) / w4;
			rotation.Y = (forward.X - right.Z) / w4;
			rotation.Z = (right.Y - cameraUp.X) / w4;

			return rotation;
		}

		public static Matrix4 CreateLookAt(Vector3 cameraPosition, Vector3 targetPosition, Vector3 upVector)
		{
			Vector3 forward = (targetPosition - cameraPosition).Normalized();
			forward = -forward;

			Vector3 right = Vector3.Cross(upVector, forward).Normalized();
			Vector3 up = Vector3.Cross(forward, right);

			Matrix4 lookAtMatrix = new Matrix4();

			lookAtMatrix[0, 0] = right.X;
			lookAtMatrix[1, 0] = right.Y;
			lookAtMatrix[2, 0] = right.Z;
			lookAtMatrix[3, 0] = 0.0f;

			lookAtMatrix[0, 1] = up.X;
			lookAtMatrix[1, 1] = up.Y;
			lookAtMatrix[2, 1] = up.Z;
			lookAtMatrix[3, 1] = 0.0f;

			lookAtMatrix[0, 2] = forward.X;
			lookAtMatrix[1, 2] = forward.Y;
			lookAtMatrix[2, 2] = forward.Z;
			lookAtMatrix[3, 2] = 0.0f;

			lookAtMatrix[0, 3] = 0.0f;
			lookAtMatrix[1, 3] = 0.0f;
			lookAtMatrix[2, 3] = 0.0f;
			lookAtMatrix[3, 3] = 1.0f;

			Matrix4 TranslationMatrix = Matrix4.Identity;
			TranslationMatrix[0, 3] = -cameraPosition.X;
			TranslationMatrix[1, 3] = -cameraPosition.Y;
			TranslationMatrix[2, 3] = -cameraPosition.Z;

			return lookAtMatrix * TranslationMatrix;
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