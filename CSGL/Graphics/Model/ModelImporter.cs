using Assimp;

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

		public Mesh rootMesh;

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
		}

		Mesh processNode(Node node, Scene scene)
		{
			Mesh currentMesh = new Mesh(node.Name);

			
			if (node.MeshCount > 0)
			{
				foreach (int meshIndex in node.MeshIndices)
				{
					Mesh mesh = processMesh(scene.Meshes[meshIndex], scene);
					meshes.Add(mesh);
				}
			}

			for (int i = 0; i < node.ChildCount; i++)
			{
				Mesh child = processNode(node.Children[i], scene);
				child.parent = currentMesh;
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

			Mesh nmesh = new Mesh(vertices.ToArray(), indices.ToArray(), textures, mesh.Name);

			return nmesh;
		}

		public Model ToModel()
		{
			return new Model();
		}

		public static Model Import(string filePath)
		{
			ModelImporter importer = new ModelImporter(filePath);

			return null;
		}
	}
}
