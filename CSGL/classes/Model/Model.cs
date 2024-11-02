using OpenTK.Mathematics;
using System;
using System.Security.Cryptography.X509Certificates;

namespace CSGL
{
	public class Model
	{
		public Mesh[] Submesh;
		public string Name;

		public Model(Mesh[] submesh, string name) 
		{
			this.Submesh = submesh;
			this.Name = name;
			//this.renderObject = new MeshRenderer(this, MaterialManager.GetMaterial("default"), OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
		}

		public void Dispose()
		{
			foreach (Mesh mesh in Submesh)
			{
				mesh.Dispose();
			}
		}

		public override string ToString()
		{
			return new string($"{this.Name}");
		}
	}

	
}