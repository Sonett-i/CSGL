using Logging;
using CSGL.Engine;
using OpenTK.Mathematics;
using CSGL.Assets;
using CSGL.Graphics;
using ContentPipeline;
using SharedLibrary;
using OpenTK.Graphics.OpenGL;

namespace CSGL
{
	public class InstanceScene : Scene
	{

		public InstanceScene(string name) : base(name)
		{
			SceneManager.ActiveScene = this;
		}

		List<Matrix4> transformList = new List<Matrix4>();

		Shader shader;
		VAO vao;
		EBO ebo;
		VBO vbo;
		VBO ibo;

		Instance testInstance;
		int numInstances = 100000;

		int vec4Size = 4 * sizeof(float);
		int mat4Size = 16 * sizeof(float);

		void InitScene()
		{
			MainLight = new Light(Color4.White, 0.2f, 1f, 0.2f, 16f);
			//Box box = new Box();

			//box.transform.position = new Vector3(0, 0, 0);
			//this.renderScene.Add(box);
		}

		void InitBuffers()
		{
			shader = ShaderManager.Shaders["instance.shader"];

			ModelImporter importer = new ModelImporter(Manifest.GetAsset<ModelAsset>("Bush.fbx").FilePath);

			testInstance = new Instance(importer.meshes[0].VAO, importer.meshes[0].VBO, importer.meshes[0].EBO, ShaderManager.Shaders["instance.shader"], transformList);
		}

		public override void Awake()
		{
			Log.Info($"{base.Name}({base.sceneID}) Scene Awake");

			//Mesh mesh = new Mesh(test.m)

			base.Awake();
		}

		public override void Start()
		{
			for (int i = 0; i < numInstances; i++)
			{
				Transform transform = new Transform();

				float x = MathU.Random(0, 100);
				float y = MathU.Random(0, 100);
				float z = MathU.Random(0, 100);

				transform.rotation = Quaternion.FromEulerAngles(x, y, z);
				transform.scale = new Vector3(x/100, y/100, z/100);

				transform.position = new Vector3(x, y, z);
				transformList.Add(transform.TRS());
			}

			InitScene();
			InitBuffers();
			MainLight.transform.position = new Vector3(0.0f, 0.5f, 0.0f);
			MainLight.transform.scale = Vector3.One * 0.05f;

			Camera.main.Yaw = 0;
			Camera.main.Pitch = 0;
			Camera.main.transform.position = new Vector3(0, 0, 5);

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
			testInstance.Draw();
		}
	}
}