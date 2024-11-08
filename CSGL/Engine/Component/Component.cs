using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL
{
	public abstract class Component
	{
		public virtual void Start() { }
		public virtual void Update() { }
		public GameObject GameObject { get; set; }
	}
}
