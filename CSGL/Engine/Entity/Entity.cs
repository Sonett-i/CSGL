using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL.Engine
{
	public abstract class Entity
	{
		// Add component
		// Get component mwthods

		public List<Component> Components = new List<Component>();

		public void OnRender()
		{

		}

		public void AddComponent(Component component)
		{
			component.ParentEntity = this;
			Components.Add(component);
		}

		// Adds a component to this monobehaviour, using an initializing method (if applicable)
		public T AddComponent<T>(Action<T>? initializer = null) where T : Component, new()
		{
			T component = new T
			{
				ParentEntity = this
			};
			Components.Add(component);
			initializer?.Invoke(component);
			component.Start();
			return component;
		}

		// Return component from list, or create and add one if it does not exist
		public T GetComponent<T>(Action<T>? initializer = null) where T : Component, new()
		{
			// Look for the component in the existing list
			foreach (Component component in Components)
			{
				if (component is T matchingComponent)
					return matchingComponent;
			}

			// If not found, add a new component
			return AddComponent(initializer);
		}
	}
}
