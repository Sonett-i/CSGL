using Logging;
using CSGL.Engine;
using OpenTK.Mathematics;
using CSGL.Assets;
using CSGL.Graphics;

namespace CSGL
{
	public class DebugScene : Scene
	{

		public DebugScene(string name) : base(name)
		{
			SceneManager.ActiveScene = this;
		}

		List<Matrix4> transformList = new List<Matrix4>();

		VAO vao;
		EBO ebo;
		VBO vbo;
		VBO instancedVBO;

		void InitScene()
		{
			MainLight = new Light(Color4.White, 0.2f, 1f, 0.2f, 16f);
			
			Box box = new Box();

			box.transform.position = new Vector3(0, 0, 0);
			this.renderScene.Add(box);
		}

		public override void Awake()
		{
			Log.Info($"{base.Name}({base.sceneID}) Scene Awake");

			//Mesh mesh = new Mesh(test.m)

			base.Awake();
		}

		public override void Start()
		{
			InitScene();
			MainLight.transform.position = new Vector3(0.0f, 0.5f, 0.0f);
			MainLight.transform.scale = Vector3.One * 0.05f;

			Camera.main.Yaw = 270;
			Camera.main.Pitch = -65.5f;
			Camera.main.transform.position = new Vector3(0, 10, 5);

			for (int i = 0; i < 100; i++)
			{
				float x = MathU.Random(0, 100);
				float y = MathU.Random(0, 100);
				float z = MathU.Random(0, 100);

				Vector3 pos = new Vector3(x, y, z);
				transformList.Add(Matrix4.CreateTranslation(pos));
			}

			base.Start();
		}

		float t = 0;
		public override void Update()
		{
			t += 1;

			MainLight.transform.position += new Vector3(MathF.Cos(MathU.Rad(t)), MathF.Sin(MathU.Rad(t+10)), MathF.Sin(MathU.Rad(t))) * Time.deltaTime;

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