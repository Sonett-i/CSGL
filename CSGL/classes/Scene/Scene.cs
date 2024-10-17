using System;
using OpenTK.Mathematics;
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

			this.camera = new Camera(new Vector3(0.0f, 0.0f, 0.0f), ProjectionType.PROJECTION_PROJECTION, 0.1f, 10f, 45f);

			Camera.main = this.camera;
		}

		public void AddObjectToScene(RenderObject obj)
		{
			sceneObjects.Add(obj);
		}

		void InitializeObjects()
		{
			RenderObject quad = ObjectFactory.CreateCube(new Vector3(0.0f, 0f, 0.1f), 1f, ShaderManager.GetShader("default"), new Color4(0.5f, 0.7f, 0.3f, 1.0f));

			AddObjectToScene(quad);
		}

		public void Start()
		{
			Log.Default($"{this.Name} scene started");
			InitializeObjects();
		}

		public void Update()
		{

		}

		public void Render()
		{
			Matrix4 view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
			Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(camera.FOV), Viewport.Width / Viewport.Height, camera.NearClip, camera.FarClip);
			Matrix4 model = Matrix4.Identity; // * Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(10.0f * Time.time)) * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(10.0f * Time.time)) * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(10.0f * Time.time));

			foreach (RenderObject obj in sceneObjects)
			{
				obj.shaderProgram.SetUniform("model", model);
				obj.shaderProgram.SetUniform("view", model);
				obj.shaderProgram.SetUniform("projection", model);

				obj.Render();
			}
		}
	}
}
