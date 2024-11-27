using Logging;
using CSGL.Engine;
using OpenTK.Mathematics;
using CSGL.Assets; 

namespace CSGL
{
	public class TerrainTest : Scene
	{

		public TerrainTest(string name) : base("TerrainTest")
		{
			SceneManager.ActiveScene = this;
		}

		void InitScene()
		{
			Terrain terrain = new Terrain("heightmap.png");

			this.renderScene.Add(terrain);
		}

		public override void Awake()
		{
			base.Awake();
		}

		public override void Start()
		{
			Camera.main.Yaw = 270;
			Camera.main.Pitch = -65.5f;
			Camera.main.transform.position = new Vector3(0, 10, 5);

			MainLight.transform.position = new Vector3(-1, 1, 0);
			InitScene();

			base.Start();
		}

		float h = 0;
		public override void Update()
		{
			h += Time.deltaTime;

			//MainLight.transform.position *= new Vector3(MathF.Cos(MathU.Rad(h)), MathF.Sin(MathU.Rad(h)), 0f);

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