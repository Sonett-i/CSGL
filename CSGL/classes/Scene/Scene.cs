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

		public List<Monobehaviour> sceneGameObjects = new List<Monobehaviour>();

		public float lastUpdateTime = 0;
		public float lastRenderTime = 0;

		public Cubemap cubemap;

		public Scene(string name)
		{
			this.ID = SceneManager.Scenes.Count + 1;
			this.Name = name;

			this.camera = new Camera(new Vector3(0.0f, 0.0f, -8.0f), ProjectionType.PROJECTION_PROJECTION, 0.1f, 1000f, 45f);

			if (!SceneManager.isReloading)
				Camera.main = this.camera;
		}


		public void AddObjectToScene(GameObject obj)
		{
			sceneGameObjects.Add(obj);
		}

		void InitializeObjects()
		{
			//Windmill wm = new Windmill(new Transform(new Vector3(0, 0, 0), Quaternion.Identity, Vector3.One));
			GameObject map = new GameObject(new Transform(), new MeshRenderer(ModelManager.LoadModel("MainMap.obj"), MaterialManager.GetMaterial("textured")));
			map.Transform.Scale *= 15f;

			for (int i = 0; i < 5; i++)
			{
				float x = CSGLU.Random(-20, 20);
				float z = CSGLU.Random(-50, 50);
				Vector3 randomVector = new Vector3(x, 0, z);

				Windmill wm = new Windmill(new Transform(randomVector, Quaternion.Identity, Vector3.One * 0.5f));
			}
			
			//sceneGameObjects.Add(go);
			//sceneGameObjects.Add(wm);

			this.cubemap = new Cubemap();

			foreach (Monobehaviour obj in Monobehaviour.Monobehaviours)
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

			foreach (Monobehaviour obj in Monobehaviour.Monobehaviours)
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

			foreach (Monobehaviour obj in Monobehaviour.Monobehaviours)
			{
				obj.Update();
			}

			float end = Time.time;

			lastUpdateTime = end - start;
		}

		public void Render()
		{
			float start = Time.time;

			foreach (Monobehaviour obj in Monobehaviour.Monobehaviours)
			{
				obj.OnRender();
			}

			float end = Time.time;

			if (cubemap != null)
				cubemap.Draw(Camera.main.m_View, Camera.main.m_Projection);
			
			lastRenderTime = end - start;
		}

		public void Unload()
		{
			foreach (Monobehaviour gameObject in sceneGameObjects)
			{
				gameObject.MeshRenderer.Dispose();
			}

			if (cubemap != null)
				cubemap.Dispose();

			sceneGameObjects.Clear();
		}
	}
}
