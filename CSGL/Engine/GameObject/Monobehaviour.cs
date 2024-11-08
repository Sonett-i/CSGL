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

		public Monobehaviour()
		{

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

		}

		public void AddComponent(Component component)
		{
			Components.Add(component);
		}

		public T AddComponent<T>(Action<T> initializer = null) where T : Component, new()
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

		public T GetComponent<T>() where T : Component
		{
			foreach (Component component in Components)
			{
				if (component is T matchingComponent)
					return matchingComponent;
			}
			return null;
		}
	}
}
