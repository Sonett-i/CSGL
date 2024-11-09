using System;
using System.Reflection;
using CSGL.Math;
using OpenTK.Mathematics;
using System.Text.Json;

namespace CSGL
{
	public class MeshFilter : Component
	{
		public string Name = "";
		public string modelname = "";
		public Mesh[]? Meshes;
		public int Size = 0;

		public MeshFilter() { }

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

		public void Set(MeshFilter meshFilter)
		{
			this.Name = meshFilter.Name;
			this.Meshes = meshFilter.Meshes;
			this.Size = meshFilter.Size;
		}

		public override void Instance(Monobehaviour parent, Dictionary<string, JsonElement> serialized)
		{
			string modelname = "";

			foreach (KeyValuePair<string, JsonElement> property in serialized)
			{
				if (property.Key == "modelname")
				{
					modelname = property.Value.GetString() ?? "cube.obj";
				}
			}

			MeshFilter mf = Resources.MeshFilters[modelname];
			
			this.Name = mf.Name;
			this.Meshes = mf.Meshes;
			this.Size = mf.Size;
		}
	}
}
