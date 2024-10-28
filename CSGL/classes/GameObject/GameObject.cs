using System;

namespace CSGL
{
	public class GameObject : Monobehaviour
	{
		int ID;


		public GameObject(Transform transform, RenderObject renderObject) : base(transform, renderObject)
		{

		}

		public override void Start()
		{

			base.Start();
		}

		public override void Update()
		{
			base.Update();
		}

		// To-do instantiation with location, transforms etc
		public static T Instantiate<T>(T original, MainWindow mainwindow) where T : GameObject, new()
		{
			T instance = new T();

			instance.Transform.Position = original.Transform.Position;
			instance.Transform.Rotation = original.Transform.Rotation;
			instance.Transform.Scale = original.Transform.Scale;

			mainwindow.scene.AddObjectToScene(instance);
			instance.Start();

			return instance;
		}


		public void Render()
		{
			RenderObject.Render();
		}
	}
}
