using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

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

		public Scene(string name)
		{
			this.ID = SceneManager.Scenes.Count + 1;
			this.Name = name;

			this.camera = new Camera(new Vector3(0.0f, 0.0f, -8.0f), ProjectionType.PROJECTION_PROJECTION, 0.1f, 10000f, 45f);

			player = new Player();

			if (!SceneManager.isReloading)
				Camera.main = this.camera;
		}

		// Add objects to the scene to be updated and rendered each frame
		public void AddObjectToScene(GameObject obj)
		{
			obj.Scene = this;
			sceneGameObjects.Add(obj);
		}

		void InitializeObjects()
		{
			/*
			this.cubemap = new Cubemap(new string[]{ 
				"nebula_right.jpg",
				"nebula_left.jpg",
				"nebula_top.jpg",
				"nebula_bottom.jpg",
				"nebula_front.jpg",
				"nebula_back.jpg",
			});
			*/

			//this.cubemap = new Cubemap();

			this.cubemap = new Cubemap(new string[]{
				"starry-top.jpg",
				"starry-right.jpg",
			});


			player.OnAwake();
			Camera.main.SetTarget(player);

			Moon moon = new Moon();
			sceneGameObjects.Add(moon);

			Satellite satellite = new Satellite();

			satellite.Transform.LocalScale = Vector3.One * 10f;
			satellite.SetMoon(moon);

			sceneGameObjects.Add(satellite);

			player.SetOrbit((Monobehaviour)moon);

			foreach (Monobehaviour obj in sceneGameObjects)
			{
				obj.OnAwake();
			}
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

		// Render each object in the scene
		public void Render()
		{
			float start = Time.time;

			player.OnRender();

			foreach (Monobehaviour obj in sceneGameObjects)
			{
				obj.OnRender();
			}

			float end = Time.time;

			// Draw the cubemap last (if not null)
			if (cubemap != null)
				cubemap.Draw();
			
			lastRenderTime = end - start;
		}

		// Remove objects from memory when the scene unloads
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
