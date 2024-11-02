using System;
using OpenTK.Mathematics;

namespace CSGL
{
	public static class OBJ
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


		public static Model Import(string[] data, string fileName)
		{
			List<Mesh> meshes = new List<Mesh>();

			int submeshIndex = -1;

			string o = "";
			int s = 0;

			List<Submesh> submesh = new List<Submesh>();

			for (int i = 2; i < data.Length; i++)
			{
				string[] line = data[i].Split(' ');

				switch (line[0])
				{
					case "o":
						submeshIndex++;
						submesh.Add(new Submesh(submeshIndex));
						submesh[submeshIndex].o = line[1];
						break;

					case "v":
						submesh[submeshIndex].vertices.Add(new Vertex(Vector3FromString(line)));
						break;

					case "vn":
						submesh[submeshIndex].normals.Add(new VertexNormal(Vector3FromString(line)));
						break;

					case "vt":
						submesh[submeshIndex].texCoords.Add(new TextureCoordinate(Vector2FromString(line)));
						break;

					case "s":
						submesh[submeshIndex].s = line[1];
						break;

					case "mtllib":
						break;

					case "usemtl":
						break;

					case "f":
						Vector3i[] face = new Vector3i[line.Length - 1];
						int faceIndex = 0;

						for (int j = 1; j < line.Length; j++)
						{
							string[] faces = line[j].Split("/");
							face[faceIndex] = Vector3iFromString(faces);
							faceIndex++;
						}
						Face modelFace = new Face(face);
						submesh[submeshIndex].Faces.Add(modelFace);
						break;
				}
			}

			Log.Default("submeshes" + submesh.ToString());

			foreach (Submesh sm in submesh)
			{
				meshes.Add(sm.ToMesh());
			}

			Model model = new Model(meshes.ToArray(), fileName);

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

