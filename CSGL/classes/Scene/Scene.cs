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

		public Scene(string name) 
		{
			this.ID = -1;
			this.Name = name;

			this.camera = new Camera(new Vector3(0.0f, 0.0f, -3.0f), ProjectionType.PROJECTION_PROJECTION, 0.1f, 10f, 45f);

			Camera.main = this.camera;
		}

		public void AddObjectToScene(RenderObject obj)
		{
			sceneObjects.Add(obj);
		}

		void InitializeObjects()
		{
			//RenderObject test = ObjectFactory.CreateCube(new Vector3(0.0f, 0f, 0.1f), 1f, ShaderManager.GetShader("default"), new Color4(0.5f, 0.7f, 0.3f, 1.0f));

			RenderObject test = ModelManager.LoadModel("Cube").renderObject;

			AddObjectToScene(test);
		}

		public void Start()
		{
			Log.Default($"{this.Name} scene started");
			InitializeObjects();
		}

		public void Update()
		{
			Vector3 position = new Vector3(Input.GetAxisRaw("Horizontal").X, 0, Input.GetAxisRaw("Vertical").Y);

			Camera.main.Position += position * Time.deltaTime;
		}

		public void Render()
		{
			foreach (RenderObject obj in sceneObjects)
			{
				obj.Render();
			}
		}
	}
}
