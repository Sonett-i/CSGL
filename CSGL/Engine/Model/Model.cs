using System;
using ContentPipeline;
using ContentPipeline.Components;

namespace CSGL.Engine
{
	public class Model : Component, IDisposable
	{
		public List<Mesh> meshes = new List<Mesh>();
		public List<Texture> textures = new List<Texture>();

		public Model()
		{

		}

		public Model(ModelAsset modelAsset, List<Texture> textures)
		{
			this.textures = textures;

			for (int i = 0; i < modelAsset.Meshes.Length; i++)
			{
				Mesh mesh = new Mesh(modelAsset.Meshes[i].Vertices, modelAsset.Meshes[i].Indices, this.textures, ShaderManager.Shaders["default.shader"]);
				meshes.Add(mesh);
			}
		}

		public void AddMesh(Mesh mesh)
		{
			meshes.Add(mesh);
		}

		public void Draw()
		{
			for (int i = 0; i < meshes.Count; i++)
			{
				meshes[i].Draw(meshes[i].Shader, Camera.main, ParentEntity.transform.Transform_Matrix);
			}
		}

		~Model()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			foreach (Mesh mesh in meshes)
			{
				mesh.Dispose();
			}
		}
	}
}
