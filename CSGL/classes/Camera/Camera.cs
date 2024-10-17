using System;
using OpenTK.Mathematics;

namespace CSGL
{
	public enum ProjectionType
	{
		PROJECTION_ORTHOGRAPHIC,
		PROJECTION_PROJECTION
	}

	public class Camera
	{
		public static Camera main = new Camera(Vector3.Zero, ProjectionType.PROJECTION_PROJECTION, 0.1f, 10.0f, 45f);

		public Vector3 Position;
		public ProjectionType ProjectionType;

		public float NearClip;
		public float FarClip;
		public float FOV;

		public Camera(Vector3 position, ProjectionType projectionType, float nearClip, float farClip, float fov)
		{
			Position = position;
			ProjectionType = projectionType;
			NearClip = nearClip;
			FarClip = farClip;
			FOV = fov;
		}
	}
}
