using System;
using OpenTK.Mathematics;

namespace CSGL
{
	public static class Obj
	{
		/*	obj format
		 *	
		 *	o header
		 *	v	x y z	|	vertex
		 *	vn	x y z	|	vertex normals
		 *	vt	u v		|	vertex texture coordinate
		 *	s	f		|	sides
		 *	f	v/vt/vn	|	face definitions
		 */

		public static Model Import(string[] data)
		{
			List<Vertex> _v = new List<Vertex>();
			List<VertexNormal> _vn = new List<VertexNormal>();
			List<TextureCoordinate> _vt = new List<TextureCoordinate>();
			List<Face> _f = new List<Face>();

			string o = "";
			int s = 0;

			for (int i = 2; i < data.Length; i++)
			{
				string[] line = data[i].Split(' ');

				if (line[0] == "o")
				{
					o = line[1];
				}

				if (line[0] == "v")
				{
					Vertex v = new Vertex(Vector3FromString(line));
					_v.Add(v);
				}

				if (line[0] == "vn")
				{
					VertexNormal vn = new VertexNormal(Vector3FromString(line));
					_vn.Add(vn);
				}

				if (line[0] == "vt")
				{
					TextureCoordinate vt = new TextureCoordinate(Vector2FromString(line));
					_vt.Add(vt);
				}

				if (line[0] == "s")
				{
					s = int.Parse(line[1]);
				}

				if (line[0] == "f")
				{
					Vector3i[] face = new Vector3i[line.Length-1];
					int faceIndex = 0;

					for (int j = 1; j < line.Length; j++)
					{
						string[] faces = line[j].Split("/");
						face[faceIndex] = Vector3iFromString(faces);

						faceIndex++;
					}

					Face modelFace = new Face(face);
					_f.Add(modelFace);
				}
			}

			Model model = new Model(_v.ToArray(), _vn.ToArray(), _vt.ToArray(), _f.ToArray(), o, s);

			return model;

		}

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
			int x = int.Parse(data[0]) -1;
			int y = int.Parse(data[1]) -1;
			int z = int.Parse(data[2]) -1;

			return new Vector3i(x, y, z);
		}
	}
}



// https://en.wikipedia.org/wiki/Wavefront_.obj_file

// https://cs418.cs.illinois.edu/website/text/obj.html

// https://cs.wellesley.edu/~cs307/readings/obj-ojects.html

