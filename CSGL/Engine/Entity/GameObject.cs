using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL.Engine
{
	public class GameObject : Entity
	{
		public GameObject(string name) : base(name) 
		{
			base.EntityType = EntityType.GameObject;
		}


	}
}
