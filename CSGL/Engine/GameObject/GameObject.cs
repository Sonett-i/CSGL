using System;
using System.Numerics;

namespace CSGL
{
	public class GameObject : Monobehaviour
	{
		private List<Component> components = new List<Component>();

		public T AddComponent<T>() where T : Component, new()
		{
			T component = new T
			{
				GameObject = this
			};
			components.Add(component);
			component.Start();
			return component;
		}

		public T GetComponent<T>() where T : Component
		{
			foreach (var component in components)
			{
				if (component is T matchingComponent)
					return matchingComponent;
			}
			return null;
		}

		public GameObject(Transform transform, MeshRenderer meshRenderer) : base()
		{

		}

		public override void Start()
		{
			base.Start();
		}

		public override void Update()
		{
			foreach(var component in components)
			{ 
				component.Update(); 
			}

			base.Update();
		}

		public override void OnRender()
		{
			base.OnRender();
		}
	}
}
