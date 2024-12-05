using System;
using CSGL.Graphics;
using Logging;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace CSGL.Engine
{
	public enum CameraType
	{
		FollowCamera,
		FreeCamera
	}

	public class Camera : Entity
	{
		public static Camera main = null!;

		public CameraType cameraType;

		// Camera Details
		public float NearClip;
		public float FarClip;
		public float FOV;

		Entity target = null!;

		private Vector3 front = -Vector3.UnitZ;
		public Vector3 Front => front;
		private Vector3 up = Vector3.UnitY;
		public Vector3 Up => up;

		private Vector3 right = Vector3.UnitX;
		public Vector3 Right => right;

		public Vector3 Direction => (transform.position + Front);

		Vector3 targetLook = Vector3.Zero;

		// Cached matrices
		public Matrix4 ViewMatrix { get; set; }
		public Matrix4 ProjectionMatrix { get; set; }

		public float MovementSpeed = 23.5f;
		public float Sensitivity = 0.30f;

		// Smoothing
		float smoothing = 0.5f;
		Vector2 smoothedDelta = Vector2.Zero;
		Vector3 smoothedMovement = Vector3.Zero;

		Vector2 lastMouse = Vector2.Zero;
		bool firstMove = true;

		Vector3 currentForce = Vector3.Zero;
		Vector3 previousForce = Vector3.Zero;

		Vector3 currentLook = Vector3.Zero;
		Vector3 targetCurrent = Vector3.Zero;

		public float decay = 0.95f;

		private float pitch;
		public float Pitch
		{
			get => pitch;
			set
			{
				pitch = MathU.Clamp(value, -89.0f, 89.0f);
			}
		}

		float yaw;
		public float Yaw
		{
			get => yaw;
			set
			{
				yaw = value;
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

		public Camera() : base("Camera")
		{
			this.NearClip = 0.01f;
			this.FarClip = 100.0f;
			this.FOV = 45f;
		}

		public Camera(CameraType camType, Vector3 position, float nearClip, float farClip, float fov) : base("Camera")
		{
			this.cameraType = camType;
			this.NearClip = nearClip;
			this.FarClip = farClip;
			this.FOV = fov;
		}

		public Matrix4 GetViewMatrix()
		{
			if (target == null)
				return Matrix4.LookAt(transform.position, transform.position + Front, Up);

			return Matrix4.LookAt(transform.position, targetCurrent, Up);
		}

		public Matrix4 SkyMatrix()
		{
			Matrix4 view = Matrix4.LookAt(Vector3.Zero, -Vector3.UnitZ, Up);

			// Remove translation (position) component by resetting it
			//view.Row3 = new Vector4(0, 0, 0, 1);

			return view;
		}

		public Matrix4 GetProjectionMatrix() => Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Camera.main.FOV), WindowConfig.AspectRatio, Camera.main.NearClip, Camera.main.FarClip);

		public override void Start()
		{
			if (target != null)
			{
				float targetYaw = MathU.Deg(target.transform.rotation.ToEulerAngles().Y);

				this.yaw = targetYaw + 90;
			}

		}

		void HandleMouseInput()
		{
			if (firstMove)
			{
				lastMouse = Input.Mouse.Position;
				firstMove = false;
			}
			else
			{
				Vector2 delta = Vector2.Zero;// Input.Mouse.Delta;// Vector2.Zero;// Input.Mouse.Position - Viewport.CenterScreen;

				if (Input.Mouse.RightButtonPressed)
				{
					delta = Input.Mouse.Position - lastMouse;
				}

				smoothedDelta = Vector2.Lerp(smoothedDelta, delta, smoothing);
				lastMouse = Input.Mouse.Position;

				this.Yaw += smoothedDelta.X * this.Sensitivity;
				this.Pitch += smoothedDelta.Y * this.Sensitivity;
			}

			front = new Vector3(
				(float)MathF.Cos(MathU.Rad(Pitch)) * (float)MathF.Cos(MathU.Rad(Yaw)),
				(float)MathF.Sin(MathU.Rad(Pitch)),
				(float)MathF.Cos(MathU.Rad(Pitch)) * (float)MathF.Sin(MathU.Rad(Yaw))).Normalized();

			right = Vector3.Cross(front, up).Normalized();
		}

		void HandleKeyboardInput()
		{
			float vertical = 0;

			Vector3 forward = this.Front * Input.GetAxisRaw("Vertical");
			Vector3 right = this.Right * Input.GetAxisRaw("Horizontal");

			Vector3 targetForce = (forward + right + (up * vertical)) * Time.deltaTime * MovementSpeed;

			if (targetForce != Vector3.Zero)
			{
				currentForce = Vector3.Lerp(previousForce, targetForce, smoothing);

				previousForce = currentForce;
			}
		}

		public void SetTarget(Entity target)
		{
			if (this.cameraType == CameraType.FollowCamera)
				this.target = target;
		}

		void FollowTarget()
		{
			if (target == null)
				return;

			float targetYaw = MathU.Deg(-target.transform.rotation.ToEulerAngles().Y) + 90;
			float targetPitch = MathU.Deg(-target.transform.rotation.ToEulerAngles().Z * 0.5f);

			this.yaw = targetYaw; //float.Lerp(this.yaw, targetYaw, 1 * Time.deltaTime);
//			this.pitch = targetPitch; // float.Lerp(this.pitch, targetPitch, 1 * Time.deltaTime);

			Vector3 desiredPosition = target.transform.position - target.transform.forward * 15f;
			desiredPosition += new Vector3(0, 4.5f, 0);

			this.transform.position = desiredPosition;
			targetCurrent = target.transform.position;
		}

		public override void Update()
		{
			HandleMouseInput();
			//HandleKeyboardInput();
			FollowTarget();
		}

		public override void FixedUpdate()
		{
			currentForce *= decay;
			this.transform.position += currentForce;
		}

		public override void Render()
		{
			this.ViewMatrix = GetViewMatrix();
			this.ProjectionMatrix = GetProjectionMatrix();
		}
	}
}
