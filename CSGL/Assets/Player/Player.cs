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

		public Player() : base("Player")
		{
			this.model = new Model(Manifest.GetAsset<ModelAsset>("planemodel.fbx"));
			this.model.ParentEntity = this;

			//this.model.Meshes["Propeller"].SetParent(this.model.Meshes["Chassis"]);

			Log.Info("player loaded");
		}

		public override void Start()
		{
			base.Start();
		}

		float angle = 0;
		public override void Update()
		{
			angle += 1;

			//this.model.Meshes["Propeller"].transform.rotation = Quaternion.FromEulerAngles(0, 0, MathU.Rad(angle)) * 5f;

			base.Update();
		}
	}
}
