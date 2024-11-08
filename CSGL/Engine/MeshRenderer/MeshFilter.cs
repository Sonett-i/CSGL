using System;
using CSGL.Math;
using OpenTK.Mathematics;

namespace CSGL
{
	public class MeshFilter : Component
	{
		public string Name = "";
		public string modelname = "";
		public Mesh[]? Meshes;
		public int Size = 0;

		public MeshFilter()
		{

		}

		public MeshFilter(string name, Mesh[] mesh) 
		{
			this.Name = name;
			this.Meshes = mesh;
		}

		public void SetModel(string model)
		{
			this.Name = model;
		}

		public override string ToString()
		{
			return $"{Name}";
		}

		public override void Instance()
		{

		}
	}
}
