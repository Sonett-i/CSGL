using System;
using ContentPipeline;
using CSGL.Engine;

namespace CSGL.Graphics
{
	public class Model : Component, IDisposable
	{
		public List<Texture> textures = new List<Texture>();
		public List<Mesh> meshes = new List<Mesh>();

		public Mesh root;
		public Shader shader;

		public Model()
		{

		}

		public Model(ModelAsset modelAsset)
		{
			this.loadModel(modelAsset.FilePath);
			this.shader = ShaderManager.Shaders["default.shader"];
			
		}


		void loadModel(string path)
		{
			ModelImporter importer = new ModelImporter(path);
			this.shader = ShaderManager.Shaders["default.shader"];
			this.meshes = importer.meshes;
		}

		public void SetShader(Shader shader)
		{

		}

		public void Draw()
		{
			foreach (Mesh mesh in meshes)
			{
				mesh.Draw(this.shader, Camera.main, this.ParentEntity.transform.Transform_Matrix);
			}
		}

		~Model()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			root?.Dispose();
		}
	}
}
