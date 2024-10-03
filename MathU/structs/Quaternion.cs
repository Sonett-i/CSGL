using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathU
{
	public struct Quaternion
	{
		public float i;
		public float j;
		public float k;
		public float w; //	Real

		public Quaternion()
		{

		}

		public Quaternion(float i, float j, float k, float w)
		{
			this.i = i;
			this.j = j;
			this.k = k;
			this.w = w;
		}

		public static Quaternion Identity = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
	}
}
