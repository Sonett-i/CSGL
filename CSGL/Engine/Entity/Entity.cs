using CSGL.Graphics;

namespace CSGL.Engine
{
	public enum EntityType
	{
		Default,
		GameObject,
		Light
	}

	public class Entity
	{
		// Add component
		// Get component mwthods

		private Dictionary<Type, Component> Components = new Dictionary<Type, Component>();
		public List<Texture> Textures = new List<Texture>();

		public string Name = "";

		public EntityType EntityType;
		public bool Lit = false;

		public Model model
		{
			get
			{
				return this.GetComponent<Model>();
			}
			set
			{
				this.RemoveComponent<Model>();
				this.AddComponent<Model>(value);
			}
		}

		public Entity(string name)
		{
			this.Name = name;

			this.AddComponent<Transform>();
			this.AddComponent<Model>();

		}

		public Transform transform => this.GetComponent<Transform>();

		public virtual void Start()
		{

		}

		public virtual void Update()
		{
			this.transform.UpdateTransforms();
		}

		public virtual void FixedUpdate()
		{

		}

		public virtual void Render()
		{
			this.model.Draw();
		}

		public void Dispose()
		{
			this.GetComponent<Model>().Dispose();
		}

		// Adds a component to this monobehaviour, using an initializing method (if applicable)

		public T AddComponent<T>(T component) where T : Component
		{
			if (Components.ContainsKey(typeof(T)))
				throw new InvalidOperationException($"Component of type {typeof(T).Name} already exists in this entity.");

			component.ParentEntity = this;
			Components[typeof(T)] = component;
			component.Start();

			return component;
		}


		public T AddComponent<T>(Action<T>? initializer = null) where T : Component, new()
		{
			if (Components.ContainsKey(typeof(T)))
				throw new InvalidOperationException($"Component of type {typeof(T).Name} already exists in this entity.");
			T component = new T { ParentEntity = this };
			initializer?.Invoke(component);
			Components[typeof(T)] = component;
			component.Start();
			return component;
		}

		public void RemoveComponent<T>() where T : Component
		{
			if (Components.ContainsKey(typeof(T)))
				Components.Remove(typeof(T));
		}

		public T GetComponent<T>(Action<T>? initializer = null) where T : Component, new()
		{
			if (Components.TryGetValue(typeof(T), out var component))
				return (T)component;

			return AddComponent(initializer);
		}

		// Checks if a component of the specified type exists
		public bool HasComponent<T>() where T : Component
		{
			return Components.ContainsKey(typeof(T));
		}

		public Entity AddTexture(Texture texture)
		{
			if (texture == null) 
				throw new ArgumentNullException(nameof(texture), "Texture cannot be null");

			Textures.Add(texture);
			return this;
		}
	}
}
