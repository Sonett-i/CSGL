using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace CSGL
{
	public abstract class Component
	{
		public static Dictionary<string, Type> ComponentTypes = new Dictionary<string, Type>();
		public virtual void Start() { }
		public virtual void Update() { }
		public Monobehaviour? Monobehaviour { get; set; }

		// When importing from json, we use instance method to pass initializing variables into each component instance
		public virtual void Instance(Monobehaviour parent, Dictionary<string, JsonElement> serialized)
		{

		}

	}
}
