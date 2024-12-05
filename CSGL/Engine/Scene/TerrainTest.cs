using Logging;
using CSGL.Engine;
using OpenTK.Mathematics;
using CSGL.Assets; 

namespace CSGL
{
	public class TerrainTest : Scene
	{
		Player player = null!;
		Cubemap cubemap = null!;

		public TerrainTest(string name) : base("TerrainTest")
		{
			SceneManager.ActiveScene = this;

		}

		void InitScene()
		{
			Terrain terrain = new Terrain("heightmap.png");

			cubemap = new Cubemap("top.jpg", "top.jpg", "top.jpg", "top.jpg", "top.jpg", "top.jpg");
			player = new Player();
		}

		public override void Awake()
		{
			base.Awake();
		}

		public override void Start()
		{
			InitScene();

			Camera.main.Yaw = 451;
			Camera.main.Pitch = -21;
			Camera.main.transform.position = new Vector3(0, 10, 5);

			MainLight.transform.position = new Vector3(-1, 1000, 0);

			player.transform.position = new Vector3(0, 30, 0);
			//Camera.main.SetTarget(player);

			
			base.Start();
		}


		public override void Update()
		{

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
			cubemap.Draw();
		}
	}
}