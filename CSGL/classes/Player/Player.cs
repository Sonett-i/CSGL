using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL
{
	public class Player : Monobehaviour
	{
		public Transform transform;

		public Player(Transform transform, Model renderobject) : base(transform, renderobject)
		{
			this.transform = transform;
		}
	}
}
