using System;
using OpenTK.Mathematics;


namespace CSGL.Engine
{
	public class Camera : Entity
	{
		public static Camera main = null!;

		// Camera Details
		public float NearClip;
		public float FarClip;
		public float FOV;

		public Transform transform;

		private Vector3 front = -Vector3.UnitZ;
		public Vector3 Front => front;
		private Vector3 up = Vector3.UnitY;
		public Vector3 Up => up;

		private Vector3 right = Vector3.UnitX;
		public Vector3 Right => right;

		public Vector3 Direction => (transform.Position + Front);


		// Cached matrices
		public Matrix4 ViewMatrix { get; set; }
		public Matrix4 ProjectionMatrix { get; set; }

		public Camera() : base("Camera")
		{
			transform = new Transform();
			this.NearClip = 0.01f;
			this.FarClip = 100.0f;
			this.FOV = 45f;
		}

		public Camera(Vector3 position, float nearClip, float farClip, float fov) : base("Camera")
		{
			this.transform = new Transform();
			this.NearClip = nearClip;
			this.FarClip = farClip;
			this.FOV = fov;
		}

		public Matrix4 GetViewMatrix() => Matrix4.LookAt(transform.Position, transform.Position + Front, Up);
		public Matrix4 GetProjectionMatrix() => Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Camera.main.FOV), WindowConfig.AspectRatio, Camera.main.NearClip, Camera.main.FarClip);


		public override void Start()
		{
			
		}

		public override void Update()
		{

		}

		public override void Render()
		{
			this.ViewMatrix = GetViewMatrix();
			this.ProjectionMatrix = GetProjectionMatrix();
		}
	}
}
