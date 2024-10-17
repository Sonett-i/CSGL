using System;
using OpenTK.Mathematics;

namespace CSGL
{
	public class Obj
	{
		string? o; // object name
		int s;

		Vector3[] v;
		Vector3[] vn;
		Vector2[] vt;


		public Obj(string[] data) 
		{
			List<Vector3> _v = new List<Vector3>();
			List<Vector3> _vn = new List<Vector3>();
			List<Vector2> _vt = new List<Vector2>();

			for (int i = 2; i < data.Length; i++)
			{
				string[] line = data[i].Split(' ');

                if (line[0] == "o")
                {
					this.o = line[1];
                }

				if (line[0] == "v")
				{
					Vector3 v = Vector3FromString(line);
					_v.Add(v);
				}

				if (line[0] == "vn")
				{
					Vector3 vn = Vector3FromString(line);
					_vn.Add(vn);
				}

				if (line[0] == "vt")
				{
					Vector2 vt = Vector2FromString(line);
					_vt.Add(vt);
				}

				if (line[0] == "s")
				{
					this.s = int.Parse(line[1]);
				}

				if (line[0] == "f")
				{
					int index = 0;
					for (int j = 1; j < line.Length; j++)
					{
						string[] faces = line[j].Split("/");
					}
					
				}
            }

			v = _v.ToArray();
			vn = _vn.ToArray();
			vt = _vt.ToArray();
		}


		public static Vector3 Vector3FromString(string[] data)
		{
			float x = float.Parse(data[1]);
			float y = float.Parse(data[2]);
			float z = float.Parse(data[3]);

			return new Vector3(x, y, z);
		}

		public static Vector2 Vector2FromString(string[] data)
		{
			float x = float.Parse(data[1]);
			float y = float.Parse(data[2]);

			return new Vector2(x, y);
		}

	}
}

class Vertex
{

}

struct VertexNormal
{

}

struct TextureCoordinate
{

}

struct Face
{
	int v;

}

// https://en.wikipedia.org/wiki/Wavefront_.obj_file

/*	obj format
 *	
 *	o header
 *	v	x y z	|	vertex
 *	vn	x y z	|	vertex normals
 *	vt	u v		|	vertex texture coordinate
 *	s	f		|	sides
 *	f	v/vt/vn	|	face definitions
 */