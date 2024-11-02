using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL
{
	public static class CSGLU
	{
		public static Vector3 Vector3FromString(string[] data)
		{
			float x = float.Parse(data[1]); // x
			float y = float.Parse(data[2]); // y
			float z = float.Parse(data[3]); // z

			return new Vector3(x, y, z);
		}

		public static Vector2 Vector2FromString(string[] data)
		{
			float x = float.Parse(data[1]);
			float y = float.Parse(data[2]);

			return new Vector2(x, y);
		}

		public static Vector3i Vector3iFromString(string[] data)
		{
			int x = int.Parse(data[0]) - 1;
			int y = int.Parse(data[1]) - 1;
			int z = int.Parse(data[2]) - 1;

			return new Vector3i(x, y, z);
		}
	}
}
