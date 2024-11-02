using System;
using CSGL.Math;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CSGL
{
	public class MeshFilter
	{
		public string Name;
		public List<SubMesh> SubMeshes { get; private set; }
		public int Size;
		string Mtl = "";
		public MeshFilter(string name, int size) 
		{
			this.Name = name;
			this.Size = size;
			SubMeshes = new List<SubMesh>();
		}

		public void AddSubMesh(SubMesh subMesh)
		{
			SubMeshes.Add(subMesh);
		}

		public SubMesh GetSubMesh(int index)
		{
			return SubMeshes[index];
		}

		public override string ToString()
		{
			return $"{Name} {Size} {Mtl}: SubMeshes {SubMeshes.Count}";
		}
	}

	public struct SubMesh
	{
		public Vertex[] Vertices;
		public VertexNormal[] Normals;
		public TextureCoordinate[] UVs;
		public Face[] Faces;
		public string o;
		public int s;

		public SubMesh(Vertex[] vertices, VertexNormal[] normals, TextureCoordinate[] uvs, Face[] faces, string o, int s)
		{
			Vertices = vertices;
			Normals = normals;
			UVs = uvs;
			Faces = faces;
			this.o = o;
			this.s = s;
		}
	}
}
