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

		public Transform Transform;

		public Vector3 Position;
		public ProjectionType ProjectionType;

		public float NearClip;
		public float FarClip;
		public float FOV;

		public Matrix4 m_Projection;
		public Matrix4 m_View;

		public Camera(Vector3 position, ProjectionType projectionType, float nearClip, float farClip, float fov)
		{
			this.Transform = new Transform(position, Quaternion.Identity, Vector3.One);
			Position = position;
			ProjectionType = projectionType;
			NearClip = nearClip;
			FarClip = farClip;
			FOV = fov;
		}

		public void Update()
		{
			m_View = Matrix4.CreateTranslation(Camera.main.Position.X, Camera.main.Position.Y, Camera.main.Position.Z);
			m_Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Camera.main.FOV), Viewport.Width / Viewport.Height, Camera.main.NearClip, Camera.main.FarClip);
		}
	}
}
