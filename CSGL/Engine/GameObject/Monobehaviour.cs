using System;
using OpenTK;
using OpenTK.Mathematics;
using System.Reflection;

namespace CSGL
{
	public class Monobehaviour
	{
		public static Dictionary<string, Type> ObjectTypes = new Dictionary<string, Type>(); // = Monobehaviour.GetDerivedTypesDictionary<Monobehaviour>();
		public static List<Monobehaviour> Monobehaviours = new List<Monobehaviour>();
		public static List<Monobehaviour> GameObjects = new List<Monobehaviour>();

		public List<Component> Components = new List<Component>();

		public Scene? Scene { get; set; }

		public Transform Transform;

		public Monobehaviour()
		{
			Transform = new Transform();
		}

		public virtual void OnAwake()
		{
			Log.Default($"{this.GetType()} awake");
			//RenderObject.m_Model = MathU.TRS(Transform);
		}

		public virtual void Start()
		{

		}

		public virtual void Update()
		{
			foreach (var component in Components)
			{
				component.Update();
			}
		}

		public virtual void FixedUpdate()
		{

		}

		public virtual void OnRender()
		{
			this.GetComponent<MeshRenderer>().Render();
		}

		public void AddComponent(Component component)
		{
			component.Monobehaviour = this;
			Components.Add(component);
		}

		public T AddComponent<T>(Action<T>? initializer = null) where T : Component, new()
		{
			T component = new T
			{
				Monobehaviour = this
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
