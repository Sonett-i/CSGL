using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static System.Formats.Asn1.AsnWriter;

namespace CSGL
{
	public class Scene
	{
		public int? ID = -1;
		public string? Name = "";
		public string? FilePath = "";

		public List<Monobehaviour> sceneGameObjects = new List<Monobehaviour>();

		public float lastUpdateTime = 0;
		public float lastRenderTime = 0;

		public Camera camera;
		public Cubemap? cubemap;
		public Player player;

		bool initialized = false;
		public Scene(string name)
		{
			this.ID = SceneManager.Scenes.Count + 1;
			this.Name = name;

			this.camera = new Camera(new Vector3(0.0f, 0.0f, -8.0f), ProjectionType.PROJECTION_PROJECTION, 0.1f, 1000f, 45f);

			player = new Player();

			if (!SceneManager.isReloading)
				Camera.main = this.camera;
		}

		public void AddObjectToScene(GameObject obj)
		{
			obj.Scene = this;
			sceneGameObjects.Add(obj);
		}

		void InitializeObjects()
		{
			this.cubemap = new Cubemap();

			player.OnAwake();
			Camera.main.SetTarget(player);
			

			Moon moon = new Moon();
			sceneGameObjects.Add(moon);

			for (int i = 0; i < 3; i++)
			{
				Satellite satellite = new Satellite();

				satellite.SetMoon(moon);
				sceneGameObjects.Add(satellite);
			}

			player.SetOrbit((Monobehaviour)moon);

			foreach (Monobehaviour obj in sceneGameObjects)
			{
				obj.OnAwake();
			}
			initialized = true;
		}

		public void Start()
		{
			Log.Default($"{this.Name} ({this.ID}) scene started");
			InitializeObjects();

			Camera.main.Transform.Position = new Vector3(0, 10, -20);

			foreach (Monobehaviour obj in sceneGameObjects)
			{
				obj.Start();
			}

		}

		public void FixedUpdate()
		{
			player.FixedUpdate();
			Camera.main.FixedUpdate();

			foreach (Monobehaviour obj in sceneGameObjects)
			{
				obj.FixedUpdate();
			}
		}

		public void Update()
		{
			float start = Time.time;
			player.Update();

			foreach (Monobehaviour obj in sceneGameObjects)
			{
				obj.Update();
			}

			Camera.main.Update();
			float end = Time.time;

			lastUpdateTime = end - start;
		}

		public void Render()
		{
			float start = Time.time;

			player.OnRender();

			foreach (Monobehaviour obj in sceneGameObjects)
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
				gameObject.GetComponent<MeshRenderer>().Dispose();
			}

			if (cubemap != null)
				cubemap.Dispose();

			
			sceneGameObjects.Clear();
		}
	}
}
