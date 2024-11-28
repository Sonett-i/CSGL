using OpenTK.Mathematics;
using Logging;

namespace CSGL.Engine
{
	public class Scene
	{
		public int sceneID = -1;
		public readonly string Name = "";

		public List<Entity> renderScene = new List<Entity>();
		public List<Light> lights = new List<Light>();

		public Light MainLight = new Light(new Color4(1f, 1f, 1f, 1f), 0.6f, 0.6f, 0.2f, 16f);

		public Camera camera;

		public Scene(string name) 
		{ 
			this.Name = name; 
			this.sceneID = SceneManager.AddScene(this);

			camera = new Camera(new Vector3(0, 0, 0), 0.01f, 10000.0f, 60);

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
			MainLight.Start();
			foreach (Entity entity in renderScene)
			{
				entity.Start();
			}
		}

		public virtual void Update()
		{
			Camera.main.Update();
			MainLight.Update();
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
				entity.Render();
			}
			//MainLight.Render();
		}

		public virtual void Unload()
		{
			foreach (Entity entity in renderScene)
			{
				entity.Dispose();
			}
		}

		public static void Instantiate(Entity entity)
		{

		}
	}
}
