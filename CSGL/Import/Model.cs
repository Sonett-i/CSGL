using System;
using OpenTK.Mathematics;
using System.IO;
using Assimp;

#pragma warning disable CS8604

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

			List<Material> materials = new List<Material>();

			// Get all object materials
			foreach (Assimp.Material mat in scene.Materials) 
			{
				string matName = mat.Name;

				if (Resources.Materials.TryGetValue(matName, out Material? material))
				{
					materials.Add(material);
				}
				else
				{
					materials.Add(Material.DefaultMaterial); // return default material if material not found
				}
			}

			for (int i = 0; i < scene.Meshes.Count; i++)
			{
				string name = scene.Meshes[i].Name;
				List<Vertex> vertices = new List<Vertex>();
				List<uint> indices = new List<uint>();
				
				Material meshMaterial = materials[scene.Meshes[i].MaterialIndex]; // get current meshes material index

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
				mesh[i] = new Mesh(vertices.ToArray(), indices.ToArray(), name, vertices.Count, scene.Meshes[i].FaceCount, meshMaterial);
			}

			MeshFilter meshFilter = new MeshFilter(Path.GetFileName(filePath), mesh);
			return meshFilter;
		}
	}
}

