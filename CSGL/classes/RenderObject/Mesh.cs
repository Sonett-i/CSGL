using System;
using OpenTK;
using OpenTK.Mathematics;

namespace CSGL
{
	public class Mesh
	{
		public string name;
		public MeshRenderer MeshRenderer;
		public Material Material;

		public Vertex[] vertices;
		public VertexNormal[] normals;
		public TextureCoordinate[] texCoords;
		public Face[] faces;

		public Mesh(Submesh submesh, Material material)
		{
			this.vertices = submesh.vertices.ToArray();
			this.normals = submesh.normals.ToArray();
			this.texCoords = submesh.texCoords.ToArray();
			this.faces = submesh.Faces.ToArray();

			this.MeshRenderer = new MeshRenderer(this, Material);
		}

		public void Dispose()
		{
			this.MeshRenderer.Dispose();
		}

	}

	// Intermediate class
	public class Submesh
	{
		public List<Vertex> vertices = new List<Vertex>();
		public List<VertexNormal> normals = new List<VertexNormal>();
		public List<TextureCoordinate> texCoords = new List<TextureCoordinate>();
		public List<Face> Faces = new List<Face>();
		public string s;
		public string o;
		public Material material;

		public int index;

		public Submesh(int index)
		{
			this.index = index;
		}

		public Mesh ToMesh()
		{
			Mesh mesh = new Mesh(this, MaterialManager.DefaultMaterial);

			return mesh;
		}
	}

	public struct Vertex
	{
		public float x;
		public float y;
		public float z;

		public Vertex(Vector3 position)
		{
			this.x = position.X;
			this.y = position.Y;
			this.z = position.Z;
		}
	}

	public struct VertexNormal
	{
		public Vector3 normal;

		public VertexNormal(Vector3 normal)
		{
			this.normal = normal;
		}
	}

	public struct TextureCoordinate
	{
		public Vector2 uv;

		public TextureCoordinate(Vector2 uv)
		{
			this.uv = uv;
		}
	}

	public struct Face
	{
		public int[] v = new int[500];
		public int[] vt = new int[500];
		public int[] vn = new int[500];

		public Face(Vector3i[] face)
		{
			v = new int[face.Length];
			vt = new int[face.Length];
			vn = new int[face.Length];

			for (int i = 0; i < face.Length; i++)
			{
				v[i] = face[i].X;
				vt[i] = face[i].Y;
				vn[i] = face[i].Z;
			}
		}
	}
}
