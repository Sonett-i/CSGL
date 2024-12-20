﻿using System;
using OpenTK.Mathematics;
using OpenTK;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

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

		Monobehaviour Target = null!;

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

		public float Speed = 15.5f;
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

		// Smoothing
		float smoothing = 0.5f;
		Vector2 smoothedDelta = Vector2.Zero;
		Vector3 smoothedMovement = Vector3.Zero;

		// Position
		Vector3 currentPosition = Vector3.Zero;
		Vector3 currentForce = Vector3.Zero;
		Vector3 previousForce = Vector3.Zero;
		float decay = 1.0f;

		// Follow
		bool toggleFollow = false;

		float orbitDistance = 1.0f;

		public Camera(Vector3 position, ProjectionType projectionType, float nearClip, float farClip, float fov)
		{
			this.Transform = new Transform(position, Quaternion.Identity, Vector3.One);
			ProjectionType = projectionType;
			NearClip = nearClip;
			FarClip = farClip;
			FOV = fov;

			Camera.main = this;
		}

		public void SetTarget(Monobehaviour target)
		{
			this.Target = target;
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

		void FollowTarget()
		{
			Vector3 target = (Target.Transform.Position - this.Transform.Position) * Time.deltaTime * orbitDistance;
			currentForce = Vector3.Lerp(currentForce, target, smoothing);

			previousForce = currentForce;
		}

		void HandleKeyboardInput()
		{
			if (Input.KeyboardState.IsKeyReleased(Keys.Space))
			{
				toggleFollow = !toggleFollow;
				Log.Default("Camera movement: " + (toggleFollow));
			}

			float vertical = 0;

			if (Input.KeyboardState.IsKeyDown(Keys.Q))
				vertical = -1;

			if (Input.KeyboardState.IsKeyDown(Keys.E))
				vertical = 1;
				

			if (toggleFollow)
			{
				Vector3 forward = this.Front * Input.GetAxisRaw("Vertical");
				Vector3 right = this.Right * Input.GetAxisRaw("Horizontal");

				Vector3 targetForce = (forward + right + (up * vertical)) * Time.deltaTime * Speed;

				if (targetForce != Vector3.Zero)
				{
					currentForce = Vector3.Lerp(previousForce, targetForce, smoothing);

					previousForce = currentForce;
				}
				else
				{
					currentForce *= decay;
				}
			}
			else
			{
				FollowTarget();
			}
		}

		void ApplyForce()
		{
			this.Transform.Position += currentForce;
		}

		public void Update()
		{
			HandleMouseInput();
			HandleKeyboardInput();
			//FollowTarget();
		}

		public void FixedUpdate()
		{
			ApplyForce();
			UpdateView();
		}

		public Matrix4 GetViewMatrix() => Matrix4.LookAt(Transform.Position, Transform.Position + Front, Up);
		public Matrix4 GetProjectionMatrix() => Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Camera.main.FOV), Viewport.AspectRatio, Camera.main.NearClip, Camera.main.FarClip);


		void UpdateView()
		{
			if (toggleFollow)
			{
				m_View = GetViewMatrix();
			}
			else
			{
				m_View = Matrix4.LookAt(Transform.Position, Target.Transform.Position, Up);
			}

			m_Projection = GetProjectionMatrix();
		}
	}
}
