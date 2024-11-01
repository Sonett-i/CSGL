using System;
using OpenTK.Mathematics;
using OpenTK;
using OpenTK.Windowing.GraphicsLibraryFramework;

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

		// Axes
		private Vector3 front = -Vector3.UnitZ;
		public Vector3 Front => front;
		private Vector3 up = Vector3.UnitY;
		public Vector3 Up => up;

		private Vector3 right = Vector3.UnitX;
		public Vector3 Right => right;

		public Vector3 Direction => (Transform.Position + Front);

		

		private float pitch;
		public float Pitch
		{
			get => pitch;
			set
			{
				pitch = MathU.Clamp(value, -89.0f, 89.0f);
				UpdateView();
			}
		}

		float yaw;
		public float Yaw
		{
			get => yaw;
			set
			{
				yaw = value;
				UpdateView();
			}
		}

		float roll;
		public float Roll
		{
			get => roll;
			set
			{
				roll = value;
			}
		}

		public float Speed = 2.0f;
		public float Sensitivity = 0.30f;


		public ProjectionType ProjectionType;

		public float NearClip;
		public float FarClip;
		public float FOV;

		public Matrix4 m_Projection;
		public Matrix4 m_View;

		private bool firstMove = false;
		Vector2 lastMouse = Input.Mouse.Position;
		Vector2 direction = new Vector2(-90.0f, 0.0f);

		public Camera(Vector3 position, ProjectionType projectionType, float nearClip, float farClip, float fov)
		{
			this.Transform = new Transform(position, Quaternion.Identity, Vector3.One);
			ProjectionType = projectionType;
			NearClip = nearClip;
			FarClip = farClip;
			FOV = fov;

			Camera.main = this;
		}

		void HandleInput()
		{
			if (firstMove)
			{
				lastMouse = Input.Mouse.Position;
				firstMove = false;
			}
			else
			{
				Vector2 delta = Input.Mouse.Position - Viewport.CenterScreen;
				lastMouse = Input.Mouse.Position;

				this.Yaw += delta.X * this.Sensitivity;
				this.Pitch += delta.Y * this.Sensitivity;
			}

			front = new Vector3(
				(float)MathF.Cos(MathU.Rad(Pitch)) * (float)MathF.Cos(MathU.Rad(Yaw)),
				(float)MathF.Sin(MathU.Rad(Pitch)),
				(float)MathF.Cos(MathU.Rad(Pitch)) * (float)MathF.Sin(MathU.Rad(Yaw))).Normalized();

			right = Vector3.Cross(front, up).Normalized();

			TranslateCamera();
		}

		public void Update()
		{
			HandleInput();
		}

		public Matrix4 GetViewMatrix() => Matrix4.LookAt(Transform.Position, Transform.Position + Front, Up);
		public Matrix4 GetProjectionMatrix() => Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Camera.main.FOV), Viewport.AspectRatio, Camera.main.NearClip, Camera.main.FarClip);

		void TranslateCamera()
		{
			Vector3 Forward = this.Front * Input.GetAxisRaw("Vertical");
			Vector3 Right = this.Right * Input.GetAxisRaw("Horizontal");

			this.Transform.Position += (Forward + Right) * Time.deltaTime * Speed;
			//Vector3 forward = this.Transform.Forward * position.Length;
			//this.Transform.Position += position * Time.deltaTime;
		}

		void UpdateView()
		{
			m_View = GetViewMatrix();
			m_Projection = GetProjectionMatrix();
		}
	}
}
