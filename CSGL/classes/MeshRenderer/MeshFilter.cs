using System;
using CSGL.Math;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CSGL
{
	public class MeshFilter
	{
		public string Name;
		public Mesh[] Meshes;
		public int Size;


		public MeshFilter(string name, Mesh[] mesh) 
		{
			this.Name = name;
			this.Meshes = mesh;
		}

		public override string ToString()
		{
			return $"{Name}";
		}
	}
}
