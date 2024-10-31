using OpenTK.Mathematics;
using System;
using System.Security.Cryptography.X509Certificates;

namespace CSGL
{
	public class Model
	{
		public Vertex[] vertices;
		public VertexNormal[] normals;
		public TextureCoordinate[] texCoords;
		public Face[] faces;
		public int sides;
		public string name;

		public RenderObject renderObject;

		public Model(Vertex[] vertices, VertexNormal[] normals, TextureCoordinate[] texCoords, Face[] faces, string name, int sides) 
		{
			this.vertices = vertices;
			this.normals = normals;
			this.texCoords = texCoords;
			this.faces = faces;
			this.name = name;
			this.sides = sides;

			this.renderObject = new RenderObject(this, MaterialManager.GetMaterial("default"), OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
		}

		public override string ToString()
		{
			return new string($"{this.name} vertices: {vertices.Length} normals: {normals.Length} texCoords: {texCoords.Length} faces: {faces.Length}");
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