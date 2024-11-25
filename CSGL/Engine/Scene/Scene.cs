using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using Logging;

namespace CSGL.Engine
{
	public class Scene
	{
		public int sceneID = -1;
		public readonly string Name = "";

		public List<Entity> renderScene = new List<Entity>();
		public List<Light> lights = new List<Light>();

		public Light MainLight = new Light();

		public Camera camera;

		public Scene(string name) 
		{ 
			this.Name = name; 
			this.sceneID = SceneManager.AddScene(this);

			camera = new Camera();

			Camera.main = camera;
			Awake(); 
		}

		public virtual void Awake()
		{
			Log.Default(Name + " scene awake");
		}

		public virtual void Start()
		{
			Camera.main.Start();
			foreach (Entity entity in renderScene)
			{
				entity.Start();
			}
		}

		public virtual void Update()
		{
			Camera.main.Update();
			foreach (Entity entity in renderScene)
			{
				entity.Update();
			}
		}

		public virtual void FixedUpdate()
		{
			Camera.main?.FixedUpdate();
			foreach (Entity entity in renderScene)
			{
				entity.FixedUpdate();
			}
		}

		public virtual void Render()
		{
			Camera.main.Render();

			foreach (Entity entity in renderScene)
			{
				entity.GetComponent<Mesh>().Draw();
			}
		}
	}
}
