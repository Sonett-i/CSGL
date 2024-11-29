using Assimp;
using Logging;
using OpenTK.Mathematics;

namespace CSGL.Graphics
{
	internal class iMesh
	{
		public List<Vertex> vertices = new List<Vertex>();
		public List<uint> indices = new List<uint>();
		public List<Texture> Textures = new List<Texture>();

		public string name;
		public iMesh(List<Vertex> vertices, List<uint> indices, string name)
		{
			this.vertices = vertices;
			this.indices = indices;
			this.name = name;
		}
	}

	internal class ModelImporter
	{
		public List<Mesh> meshes = new List<Mesh>();
		string directory;

		public MeshNode rootMesh;

		public ModelImporter(string filePath)
		{
			loadModel(filePath);
		}

		void loadModel(string path)
		{
			AssimpContext context = new AssimpContext();

			Scene scene = context.ImportFile(path, PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals | PostProcessSteps.CalculateTangentSpace);

			if (scene == null || scene.SceneFlags == SceneFlags.Incomplete || scene.RootNode == null)
			{
				return;
			}

			directory = path;

			this.rootMesh = processNode(scene.RootNode, scene);

			Log.Info("AAAA");
		}

		MeshNode processNode(Node node, Scene scene, MeshNode parent = null)
		{
			MeshNode currentMesh = new MeshNode(node.Name);
			currentMesh.TransformMatrix = ConvertToMatrix4(node.Transform);

			node.Transform.Decompose(out Vector3D scaling, out Assimp.Quaternion rotation, out Vector3D translation);
			currentMesh.Transform.position = Convert(translation);
			currentMesh.Transform.rotation = Convert(rotation);
			currentMesh.Transform.scale = Convert(scaling);

			Vector3 euler = currentMesh.Transform.rotation.ToEulerAngles();

			if (node.MeshCount > 0)
			{
				foreach (int meshIndex in node.MeshIndices)
				{
					Mesh mesh = processMesh(scene.Meshes[meshIndex], scene);
					currentMesh.Meshes.Add(mesh);
				}
			}

			for (int i = 0; i < node.ChildCount; i++)
			{
				MeshNode child = processNode(node.Children[i], scene, currentMesh);
				child.SetParent(currentMesh);
				currentMesh.Children.Add(child);
			}

			//meshes.Add(currentMesh);

			return currentMesh;
		}

		Mesh processMesh(Assimp.Mesh mesh, Scene scene)
		{
			List<Vertex> vertices = new List<Vertex>();
			List<uint> indices = new List<uint>();
			List<Texture> textures = new List<Texture>();

			// Process Vertices
			for (int i = 0; i < mesh.VertexCount; i++)
			{
				float x = mesh.Vertices[i].X;
				float y = mesh.Vertices[i].Y;
				float z = mesh.Vertices[i].Z;

				float nx = mesh.Normals[i].X;
				float ny = mesh.Normals[i].Y;
				float nz = mesh.Normals[i].Z;

				float tx = mesh.Tangents[i].X;
				float ty = mesh.Tangents[i].Y;
				float tz = mesh.Tangents[i].Z;

				float btx = mesh.BiTangents[i].X;
				float bty = mesh.BiTangents[i].Y;
				float btz = mesh.BiTangents[i].Z;

				float u = mesh.TextureCoordinateChannels[0][i].X;
				float v = mesh.TextureCoordinateChannels[0][i].Y;

				vertices.Add(new Vertex(x, y, z, nx, ny, nz, tx, ty, tz, btx, bty, btz, u, v));
			}

			// Process Indices
			for (int i = 0; i < mesh.FaceCount; i++)
			{
				foreach (int index in mesh.Faces[i].Indices)
				{
					indices.Add((uint)index);
				}
			}

			// Process Textures

			if (mesh.MaterialIndex > 0)
			{
				Material material = scene.Materials[mesh.MaterialIndex];

				textures.AddRange(LoadMaterialTextures(material, TextureType.Diffuse, "diffuse"));
				textures.AddRange(LoadMaterialTextures(material, TextureType.Specular, "specular"));
				textures.AddRange(LoadMaterialTextures(material, TextureType.Normals, "normal"));
				textures.AddRange(LoadMaterialTextures(material, TextureType.Height, "height"));
			}

			Mesh nmesh = new Mesh(vertices.ToArray(), indices.ToArray(), textures, mesh.Name);

			return nmesh;
		}

		List<Texture> LoadMaterialTextures(Material material, TextureType textureType, string typeName)
		{
			List<Texture> textures = new List<Texture>();


			return textures;
		}

		public static Model Import(string filePath)
		{
			ModelImporter importer = new ModelImporter(filePath);

			return null;
		}

		static Matrix4 ConvertToMatrix4(Assimp.Matrix4x4 assimpMatrix)
		{
			return new Matrix4(
				assimpMatrix.A1, assimpMatrix.A2, assimpMatrix.A3, assimpMatrix.A4,
				assimpMatrix.B1, assimpMatrix.B2, assimpMatrix.B3, assimpMatrix.B4,
				assimpMatrix.C1, assimpMatrix.C2, assimpMatrix.C3, assimpMatrix.C4,
				assimpMatrix.D1, assimpMatrix.D2, assimpMatrix.D3, assimpMatrix.D4
			);
		}

		static Vector3 Convert(Vector3D vector)
		{
			return new Vector3(vector.X, vector.Y, vector.Z);
		}

		static OpenTK.Mathematics.Quaternion Convert(Assimp.Quaternion quaternion)
		{
			OpenTK.Mathematics.Quaternion q = new OpenTK.Mathematics.Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

			return q;
		}
	}
}
