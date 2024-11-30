using ContentPipeline;
using CSGL.Engine;
using CSGL.Graphics;
using Logging;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace CSGL.Assets
{
	public class Player : GameObject
	{

		float minAngle = -10;
		float maxAngle = 10;

		float leftWing = 0;
		float rightWing = 0;

		float rudder = 0;
		float tail = 0;

		float MaxSpeed = 100f;

		float currentSpeed = 0;

		Vector3 currentAxis = Vector3.Zero;

		Vector3 currentForce = Vector3.Zero;
		Vector3 previousForce = Vector3.Zero;

		float smoothing = 0.5f;
		float decay = 0.9f;
		float smoothingSpeed = 10.5f;

		float rotationSpeed = 30f;


		public Player() : base("Player")
		{
			this.model = new Model(Manifest.GetAsset<ModelAsset>("planemodelsmall.fbx"), ShaderManager.Shaders["default.shader"]);
			this.model.ParentEntity = this;

			//this.model.root.Transform.Parent = this.transform;
			//this.model.Meshes["Propeller"].SetParent(this.model.Meshes["Chassis"]);

			Log.Info("player loaded");
		}

		public override void Start()
		{
			Camera.main.SetTarget(this);
			base.Start();
		}

		void HandleInput()
		{
			float roll = Input.GetArrowInput("Horizontal");
			float pitch = Input.GetArrowInput("Vertical");

			float yaw = 0;

			if (Input.KeyboardState.IsKeyDown(Keys.Q))
				yaw = 1;

			if (Input.KeyboardState.IsKeyDown(Keys.E))
				yaw = -1;

			Vector3 targetForce = new Vector3(MathU.Rad(pitch), MathU.Rad(yaw), MathU.Rad(roll)) * rotationSpeed;

			if (targetForce != Vector3.Zero)
			{
				currentForce = Vector3.Lerp(previousForce, targetForce, Time.deltaTime * smoothingSpeed);

				previousForce = currentForce;
			}

			leftWing = MathU.Rad(-roll * 10f);
			rightWing = MathU.Rad(roll * 10f);

			tail = MathU.Rad(pitch * 10);
			rudder = MathU.Rad(yaw * 10);
		}

		float angle = 0;
		public override void Update()
		{
			angle += 25;

			HandleInput();

			this.model.Meshes["Propeller"].Transform.rotation = Quaternion.FromEulerAngles(0, 0, MathU.Rad(angle)) * 25f;
			this.model.Meshes["LeftWing"].Transform.rotation = Quaternion.FromEulerAngles(leftWing, 0, 0);
			this.model.Meshes["WingRight"].Transform.rotation = Quaternion.FromEulerAngles(rightWing, 0, 0);
			this.model.Meshes["TailLeft"].Transform.rotation = Quaternion.FromEulerAngles(tail, 0, 0);
			this.model.Meshes["TailRight"].Transform.rotation = Quaternion.FromEulerAngles(tail, 0, 0);
			this.model.Meshes["TailRudder"].Transform.rotation = Quaternion.FromEulerAngles(0, -rudder, 0);
			base.Update();
		}

		public override void FixedUpdate()
		{
			//this.model.transform.rotation *= Quaternion.FromEulerAngles(currentAxis) * Time.deltaTime * 5f;
			currentForce *= decay;
			Quaternion rotationDelta = Quaternion.FromEulerAngles(currentForce * Time.deltaTime);

			this.transform.rotation *= rotationDelta;

			base.FixedUpdate();
		}
	}
}
