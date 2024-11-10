using System;
using OpenTK.Mathematics;

namespace CSGL
{
	public class Player : Monobehaviour
	{
		//string tag = "Player";

		MeshFilter meshFilter = null!;
		MeshRenderer meshRenderer = null!;

		// Camera follow mechanism
		float orbitRadius = 10.0f;
		float orbitSpeed = 1.0f;
		float orbitAngle = 0.0f;

		Monobehaviour target = null!;

		Transform leftPanels = null!;
		Transform rightPanels = null!;

		Vector3 PanelAngles = Vector3.Zero;
		Vector3 satelliteAngles = Vector3.Zero;


		// Panels

		public override void OnAwake()
		{
			meshFilter = this.GetComponent<MeshFilter>();
			meshFilter.Set(Resources.MeshFilters["Satellite.obj"]);

			meshRenderer = this.GetComponent<MeshRenderer>();
			this.meshRenderer.Create(meshFilter, Resources.Materials["default"]);

			this.Transform = new Transform();
			this.Transform.Scale *= 0.25f;

			leftPanels = new Transform();
			leftPanels.Parent = this.Transform;

			rightPanels = new Transform();
			rightPanels.Parent = this.Transform;

			Camera.main.SetTarget(this);

			RandomizeOrbit();

			base.OnAwake();
		}

		public void SetOrbit(Monobehaviour target)
		{
			this.target = target;
		}

		void RandomizeOrbit()
		{
			orbitRadius = CSGLU.Random(25, 150);
			orbitSpeed = CSGLU.Random(0.025f, 0.065f);
			orbitAngle = CSGLU.Random(-90.0f, 90.0f);
		}

		void HandleOrbit()
		{
			if (target != null)
			{
				orbitAngle += orbitSpeed * Time.deltaTime;

				float x = target.Transform.Position.X + orbitRadius * MathF.Cos(orbitAngle);
				float y = target.Transform.Position.Y + orbitRadius * MathF.Sin(orbitAngle);
				float z = target.Transform.Position.Z + orbitRadius * MathF.Sin(orbitAngle);

				this.Transform.WorldPosition = new Vector3(x, y, z);
			}
		}

		public override void Start()
		{
			this.Transform.LocalScale = Vector3.One * 0.25f;
			base.Start();
		}

		void HandleInput()
		{
			float horizontalOrbitInput = Input.GetArrowInput("Horizontal");
			float verticalOrbitInput = Input.GetArrowInput("Vertical");

			PanelAngles.X += horizontalOrbitInput * 0.1f * Time.deltaTime;
			PanelAngles.Z += verticalOrbitInput * 0.7f * Time.deltaTime;

			leftPanels.LocalRotation = Quaternion.FromEulerAngles(0, 0, PanelAngles.X);
			rightPanels.LocalRotation = Quaternion.FromEulerAngles(0, 0, PanelAngles.Z);
		}

		void OrientBox()
		{
			satelliteAngles.X += Input.GetNumpadInput("Horizontal") * 0.1f * Time.deltaTime;
			satelliteAngles.Y += Input.GetNumpadInput("Vertical") * 1f * Time.deltaTime;
			satelliteAngles.Z += Input.GetNumpadInput("Diagonal") * 1f * Time.deltaTime;

			this.Transform.LocalRotation = Quaternion.FromEulerAngles(satelliteAngles.X, satelliteAngles.Y, satelliteAngles.Z);
		}

		public override void Update()
		{
			OrientBox();
			HandleInput();
			HandleOrbit();

			base.Update();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
		}

		public override void OnRender()
		{
			this.meshRenderer.Render(new Transform[] { this.Transform, rightPanels, leftPanels });
		}
	}
}
