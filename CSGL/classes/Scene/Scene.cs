using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static System.Formats.Asn1.AsnWriter;

namespace CSGL
{
	public class Scene
	{
		int ID;
		public string Name;

		public Camera camera;
		
		public List<RenderObject> sceneObjects = new List<RenderObject>();
		public List<GameObject> sceneGameObjects = new List<GameObject>();

		public Scene(string name) 
		{
			this.ID = 0;
			this.Name = name;

			this.camera = new Camera(new Vector3(0.0f, 0.0f, -8.0f), ProjectionType.PROJECTION_PROJECTION, 0.1f, 100f, 45f);
			Camera.main = this.camera;
		}


		public void AddObjectToScene(GameObject obj)
		{
			sceneGameObjects.Add(obj);
		}

		void InitializeObjects()
		{
			//RenderObject test = ObjectFactory.CreateCube(new Vector3(0.0f, 0f, 0.1f), 1f, ShaderManager.GetShader("default"), new Color4(0.5f, 0.7f, 0.3f, 1.0f));

			RenderObject cylinder = ModelManager.LoadModel("Cylinder").renderObject;
			RenderObject cube = ModelManager.LoadModel("Cube").renderObject;
			RenderObject torus = ModelManager.LoadModel("Torus").renderObject;
			RenderObject sphere = ModelManager.LoadModel("Sphere").renderObject;
			RenderObject pyramid = ModelManager.LoadModel("Pyramid").renderObject;
			//RenderObject map = ModelManager.LoadModel("Plane").renderObject;

			GameObject worldMap = new GameObject(new Transform(new Vector3(0, 0, 0), MathU.Euler(0, 0, 0), Vector3.One), ModelManager.LoadModel("Plane").renderObject);

			GameObject textureTest = new GameObject(new Transform(new Vector3(0, 0, 0), MathU.Euler(0, 0, 0), Vector3.One), cube);

			textureTest.RenderObject.SetMaterial(MaterialManager.GetMaterial("default"));
			worldMap.RenderObject.SetMaterial(MaterialManager.GetMaterial("cloud"));

			//AddObjectToScene(box1);
			//AddObjectToScene(box2);
			AddObjectToScene(worldMap);
			AddObjectToScene(textureTest);
			

			foreach (GameObject obj in sceneGameObjects)
			{
				obj.OnAwake();
			}
		}

		public void Start()
		{
			Log.Default($"{this.Name} ({this.ID}) scene started");
			InitializeObjects();
			//Log.Default("Displaying model: " + sceneObjects[currentModel].name);

			Camera.main.Transform.Position = new Vector3(0, -10, -20);

			foreach (GameObject obj in sceneGameObjects)
			{
				obj.Start();
			}

		}

		public void FixedUpdate()
		{

		}

		public void Update()
		{
			foreach (GameObject obj in sceneGameObjects)
			{
				obj.Update();
			}

			Camera.main.Update();
		}

		public void Render()
		{
			foreach (GameObject gameObject in sceneGameObjects)
			{
				gameObject.OnRender();
			}
		}
	}
}
