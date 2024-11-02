﻿using System;

namespace CSGL
{
	public class GameObject : Monobehaviour
	{
		public GameObject(Transform transform, Model model) : base(transform, model)
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


		public override void OnRender()
		{
			base.OnRender();
		}
	}
}
