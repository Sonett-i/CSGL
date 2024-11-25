using System;
using ContentPipeline.Components;
using Assimp;
using OpenTK.Mathematics;

namespace ContentPipeline.Extensions
{
	public class Obj
	{
		public static MeshData[] Import(string filePath)
		{
			
			AssimpContext context = new AssimpContext();

			Scene scene = context.ImportFile(filePath, PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals | PostProcessSteps.CalculateTangentSpace);

			List<Assimp.Mesh> meshes = scene.Meshes;
			List<Assimp.Material> materials = scene.Materials;

			MeshData[] _meshes = new MeshData[meshes.Count];

			for (int i = 0; i < meshes.Count; i++)
			{
				List<Vertex> vertices = new List<Vertex>();

				List<uint> indices = new List<uint>();
				
				for (int j = 0; j < meshes[i].Vertices.Count; j++)
				{
					Vector3 position = new Vector3(meshes[i].Vertices[j].X, meshes[i].Vertices[j].Y, meshes[i].Vertices[j].Z);
					Vector3 tangent = new Vector3(meshes[i].Tangents[j].X, meshes[i].Tangents[j].Y, meshes[i].Tangents[j].Z);
					Vector3 normal = new Vector3(meshes[i].Normals[j].X, meshes[i].Normals[j].Y, meshes[i].Normals[j].Z);

					Vector2 uv = new Vector2(meshes[i].TextureCoordinateChannels[0][j].X, meshes[i].TextureCoordinateChannels[0][j].Y);


					vertices.Add(new Vertex(position, normal, tangent, uv));
				}

				for (int f = 0; f < scene.Meshes[i].Faces.Count; f++)
				{
					foreach (int index in scene.Meshes[i].Faces[f].Indices)
					{
						indices.Add((uint)index);
					}
				}

				_meshes[i] = new MeshData((uint)i, vertices.ToArray(), indices.ToArray(), meshes[i].FaceCount);
			}

			context.Dispose();
			return _meshes;
		}
	}
}
