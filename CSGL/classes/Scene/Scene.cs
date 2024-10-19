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

		int currentModel = 0;

		public Scene(string name) 
		{
			this.ID = -1;
			this.Name = name;

			this.camera = new Camera(new Vector3(0.0f, 0.0f, -8.0f), ProjectionType.PROJECTION_PROJECTION, 0.1f, 100f, 45f);

			Camera.main = this.camera;
		}

		public void AddObjectToScene(RenderObject obj)
		{
			sceneObjects.Add(obj);
		}

		void InitializeObjects()
		{
			//RenderObject test = ObjectFactory.CreateCube(new Vector3(0.0f, 0f, 0.1f), 1f, ShaderManager.GetShader("default"), new Color4(0.5f, 0.7f, 0.3f, 1.0f));

			RenderObject cylinder = ModelManager.LoadModel("Cylinder").renderObject;
			RenderObject cube = ModelManager.LoadModel("Cube").renderObject;
			RenderObject torus = ModelManager.LoadModel("Torus").renderObject;
			RenderObject sphere = ModelManager.LoadModel("Sphere").renderObject;
			RenderObject pyramid = ModelManager.LoadModel("Pyramid").renderObject;


			AddObjectToScene(cube);
			AddObjectToScene(cylinder);
			AddObjectToScene(torus);
			AddObjectToScene(sphere);
			AddObjectToScene(pyramid);

		}

		public void Start()
		{
			Log.Default($"{this.Name} scene started");
			InitializeObjects();
			Log.Default("Displaying model: " + sceneObjects[currentModel].name);
		}

		public void Update()
		{
			Vector3 position = new Vector3(Input.GetAxisRaw("Horizontal").X, 0, Input.GetAxisRaw("Vertical").Y);

			Camera.main.Position += position * Time.deltaTime;

			if (Input.KeyboardState.IsKeyReleased(Keys.Space))
			{
				currentModel++;

				if (currentModel >= sceneObjects.Count)
				{
					currentModel = 0;
				}

				Log.Default("Displaying model: " + sceneObjects[currentModel].name);
			}
		}

		public void Render()
		{
			sceneObjects[currentModel].Render();
		}
	}
}
