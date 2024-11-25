using CSGL.Engine;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL
{
	public class MathU
	{
		public static float Clamp(float x, float min, float max)
		{
			float t = (x < min) ? min : x;
			return (t > max) ? max : t;
		}

		public static Matrix4 TRS(Transform transform)
		{
			Matrix4 translation = Matrix4.CreateTranslation(transform.position);
			Matrix4 rotation = Matrix4.CreateFromQuaternion(transform.rotation);
			Matrix4 scale = Matrix4.CreateScale(transform.scale);

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

		public static float Rad(float degrees)
		{
			return MathF.PI * degrees / 180;
		}
	}
}
