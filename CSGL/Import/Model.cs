using System;
using OpenTK.Mathematics;
using System.IO;
using Assimp;

namespace CSGL
{
	public class Model
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

		public static MeshFilter ImportModel(string filePath)
		{
			AssimpContext context = new AssimpContext();

			Assimp.Scene scene = context.ImportFile(filePath, PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals);

			Mesh[] mesh = new Mesh[scene.Meshes.Count];

			for (int i = 0; i < scene.Meshes.Count; i++)
			{
				string name = scene.Meshes[i].Name;
				List<Vertex> vertices = new List<Vertex>();
				List<uint> indices = new List<uint>();

				for (int j = 0; j < scene.Meshes[i].VertexCount; j++)
				{
					Vertex vertex = new Vertex
					{
						Position = new Vector3(scene.Meshes[i].Vertices[j].X, scene.Meshes[i].Vertices[j].Y, scene.Meshes[i].Vertices[j].Z),
						Normal = new Vector3(scene.Meshes[i].Normals[j].X, scene.Meshes[i].Normals[j].Y, scene.Meshes[i].Normals[j].Z),
						UV = new Vector2(scene.Meshes[i].TextureCoordinateChannels[0][j].X, scene.Meshes[i].TextureCoordinateChannels[0][j].Y)
					};

					vertices.Add(vertex);
				}

				for (int f = 0; f < scene.Meshes[i].Faces.Count; f++)
				{
					foreach (int index in scene.Meshes[i].Faces[f].Indices)
					{
						indices.Add((uint)index);
					}
				}
				mesh[i] = new Mesh(vertices.ToArray(), indices.ToArray(), name, vertices.Count, scene.Meshes[i].FaceCount);
			}

			MeshFilter meshFilter = new MeshFilter(Path.GetFileName(filePath), mesh);
			return meshFilter;
		}
	}
}



// https://en.wikipedia.org/wiki/Wavefront_.obj_file

// https://cs418.cs.illinois.edu/website/text/obj.html

// https://cs.wellesley.edu/~cs307/readings/obj-ojects.html

