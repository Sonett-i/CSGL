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

		public Player() : base("Player")
		{
			this.model = new Model(Manifest.GetAsset<ModelAsset>("planemodel.fbx"));
			this.model.ParentEntity = this;

			//this.model.root.Transform.Parent = this.transform;
			//this.model.Meshes["Propeller"].SetParent(this.model.Meshes["Chassis"]);

			Log.Info("player loaded");
		}

		public override void Start()
		{
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

			currentAxis += new Vector3(MathU.Rad(pitch), MathU.Rad(yaw), MathU.Rad(roll)) * Time.deltaTime * 15f;

			leftWing = MathU.Rad(-roll * 10f);
			rightWing = MathU.Rad(roll * 10f);

			tail = MathU.Rad(pitch * 10);
			rudder = MathU.Rad(yaw * 10);
		}

		float angle = 0;
		public override void Update()
		{
			angle += 1;

			HandleInput();

			this.model.Meshes["Propeller"].Transform.rotation = Quaternion.FromEulerAngles(0, 0, MathU.Rad(angle)) * 5f;
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
			this.transform.rotation = Quaternion.FromEulerAngles(currentAxis);
			base.FixedUpdate();
		}
	}
}
