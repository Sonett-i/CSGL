using Logging;
using CSGL.Engine;
using OpenTK.Mathematics;
using CSGL.Assets; 

namespace CSGL
{
	public class TerrainTest : Scene
	{
		Player player;
		public TerrainTest(string name) : base("TerrainTest")
		{
			SceneManager.ActiveScene = this;

		}

		void InitScene()
		{
			Terrain terrain = new Terrain("heightmap.png");

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

			MainLight.transform.position = new Vector3(-1, 1, 0);

			player.transform.position = new Vector3(0, 30, 0);
			//Camera.main.SetTarget(player);


			Foliage foliage = new Foliage(new Vector3(0, 0, 0));
			
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