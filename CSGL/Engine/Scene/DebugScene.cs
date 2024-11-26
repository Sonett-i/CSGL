using Logging;
using CSGL.Engine;
using OpenTK.Mathematics;
using CSGL.Assets; 

namespace CSGL
{
	public class DebugScene : Scene
	{

		public DebugScene(string name) : base(name)
		{
			SceneManager.ActiveScene = this;
		}

		void InitScene()
		{
			Box box = new Box();
			
			this.renderScene.Add(box);
		}

		public override void Awake()
		{
			Log.Info($"{base.Name}({base.sceneID}) Scene Awake");
			InitScene();

			//Mesh mesh = new Mesh(test.m)

			base.Awake();
		}

		public override void Start()
		{
			MainLight.transform.position = new Vector3(0.5f, 0.5f, 0.5f);
			MainLight.transform.scale = Vector3.One * 0.05f;

			Camera.main.Yaw = 270;
			Camera.main.Pitch = -65.5f;
			Camera.main.transform.position = new Vector3(0, 10, 5);
			base.Start();
		}

		float t = 0;
		public override void Update()
		{
			t += 1;


			base.Update();
		}

		public override void FixedUpdate()
		{		
			base.FixedUpdate();
		}

		public override void Render()
		{
			base.Render();
		}
	}
}