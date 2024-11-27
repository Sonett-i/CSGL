using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public List<Mesh> meshes = new List<Mesh>();

		public Entity(string name)
		{
			this.Name = name;

			this.AddComponent<Transform>();
			this.AddComponent<Mesh>();

		}

		public Transform transform => this.GetComponent<Transform>();
		public Mesh mesh {
			get
			{
				return this.GetComponent<Mesh>();
			}
			set
			{
				this.RemoveComponent<Mesh>();
				this.AddComponent<Mesh>(value);
			}
		}

		public void AddMesh(Mesh mesh)
		{
			meshes.Add(mesh);
		}

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
			foreach (Mesh mesh in meshes)
			{
				mesh.Draw(mesh.Shader, Camera.main);
			}
			//this.mesh.Draw(this.mesh.Shader, Camera.main);
		}

		public void Dispose()
		{
			this.GetComponent<Mesh>().Dispose();
		}

		// Adds a component to this monobehaviour, using an initializing method (if applicable)

		public T AddComponent<T>(T component) where T : Component
		{
			component.ParentEntity = this;
			Components[typeof(T)] = component;
			component.Start();

			if (component is Mesh mesh)
				this.meshes.Add(mesh);
			return component;
		}


		public T AddComponent<T>(Action<T>? initializer = null) where T : Component, new()
		{
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
