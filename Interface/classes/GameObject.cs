using Interface.structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
	public class GameObject
	{
		public Transform transform;

		public GameObject()
		{
			this.transform = new Transform();
		}
	}
}
