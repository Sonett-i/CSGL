using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathU
{
	public struct Vector3
	{
		public float x;
		public float y; 
		public float z;

		public Vector3()
		{
			this.x = 0.0f;
			this.y = 0.0f;
			this.z = 0.0f;
		}

		public Vector3(float x, float y, float z)
		{
			this.x = x; 
			this.y = y; 
			this.z = z;
		}

		public static Vector3 Zero = new Vector3(0, 0, 0);

		public static Vector3 operator +(Vector3 left, Vector3 right)
		{
			return new Vector3(left.x + right.x, left.y + right.y, left.z + right.z);
		}

		public static Vector3 operator - (Vector3 left, Vector3 right)
		{
			return new Vector3(left.x - right.x, left.y - right.y, left.z - right.z);
		}

		public static Vector3 operator -(Vector3 left, float right)
		{
			return new Vector3(left.x - right, left.y - right, left.z - right);
		}

		public static Vector3 operator - (Vector3 left)
		{
			return new Vector3(-left.x, -left.y, -left.z);
		}
	}

	
}
