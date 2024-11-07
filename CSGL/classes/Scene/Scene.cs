using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static System.Formats.Asn1.AsnWriter;

namespace CSGL
{
	public class Scene
	{
		public int ID;
		public string Name;
		public string FilePath;

		public Camera camera;

		public List<GameObject> sceneGameObjects = new List<GameObject>();

		public float lastUpdateTime = 0;
		public float lastRenderTime = 0;

		public Cubemap cubemap;

		public Scene(string name)
		{
			this.ID = SceneManager.Scenes.Count + 1;
			this.Name = name;

			this.camera = new Camera(new Vector3(0.0f, 0.0f, -8.0f), ProjectionType.PROJECTION_PROJECTION, 0.1f, 100f, 45f);

			if (!SceneManager.isReloading)
				Camera.main = this.camera;
		}


		public void AddObjectToScene(GameObject obj)
		{
			sceneGameObjects.Add(obj);
		}

		void InitializeObjects()
		{

			//MeshRenderer meshRenderer = new MeshRenderer(ModelManager.LoadModel("cube.obj"), MaterialManager.GetMaterial("default"));
			//GameObject go = new GameObject(new Transform(new Vector3(0, 0, 0), Quaternion.Identity, Vector3.One), meshRenderer);
			MeshFilter mf = ModelManager.LoadModel("Windmill.obj");

			MeshRenderer windmill = new MeshRenderer(ModelManager.LoadModel("Windmill.obj"), MaterialManager.GetMaterial("default"));
			GameObject wm = new GameObject(new Transform(new Vector3(0, 0, 0), Quaternion.Identity, Vector3.One), windmill);

			//sceneGameObjects.Add(go);
			sceneGameObjects.Add(wm);

			this.cubemap = new Cubemap();

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

			Camera.main.Transform.Position = new Vector3(0, 10, -20);

			foreach (GameObject obj in sceneGameObjects)
			{
				obj.Start();
			}

		}

		public void FixedUpdate()
		{
			Camera.main.Update();
		}

		public void Update()
		{
			float start = Time.time;

			foreach (GameObject obj in sceneGameObjects)
			{
				obj.Update();
			}

			float end = Time.time;

			lastUpdateTime = end - start;
		}

		public void Render()
		{
			float start = Time.time;

			foreach (GameObject gameObject in sceneGameObjects)
			{
				gameObject.OnRender();
			}

			float end = Time.time;

			if (cubemap != null)
				cubemap.Draw(Camera.main.m_View, Camera.main.m_Projection);
			
			lastRenderTime = end - start;
		}

		public void Unload()
		{
			foreach (GameObject gameObject in sceneGameObjects)
			{
				gameObject.MeshRenderer.Dispose();
			}

			if (cubemap != null)
				cubemap.Dispose();

			sceneGameObjects.Clear();
		}
	}
}
