﻿using System;
using System.IO;
using ContentPipeline;
using CSGL.Engine;

namespace CSGL.Graphics
{
	public class Model : Component, IDisposable
	{
		public List<Texture> textures = new List<Texture>();
		public Dictionary<string, MeshNode> Meshes = new Dictionary<string, MeshNode>();

		public List<Mesh> _mesh = new List<Mesh>();

		public MeshNode root;

		public Shader shader;

		public Model()
		{

		}

		public Model(ModelAsset modelAsset)
		{
			ModelImporter importer = new ModelImporter(modelAsset.FilePath);

			this.shader = ShaderManager.Shaders["default.shader"];
			this._mesh = importer.meshes;

			if (root != null)
			{
				if (ParentEntity != null)
					this.root.Transform.Parent = this.ParentEntity.transform;
			}

			this.root = importer.rootMesh;

			getModelParts(root);
		}

		void getModelParts(MeshNode node)
		{
			foreach (MeshNode child in node.Children)
			{
				getModelParts(child);
				Meshes[child.Name] = child;
			}
		}

		public override void Update()
		{
			if (root != null)
			{
				root.Transform.UpdateTransforms();
			}

			base.Update();
		}

		public void Draw()
		{
			if (root != null)
			{
				root.Render(this.shader, this.ParentEntity.transform.Transform_Matrix);
			}
			else
			{
				foreach (Mesh mesh in _mesh)
				{
					mesh.Draw(this.shader, Camera.main, this.ParentEntity.transform.Transform_Matrix);
				}
			}
		}

		~Model()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			if (this.root != null)
			{
				root.Dispose();
			}
			for (int i = 0; i < _mesh.Count; i++)
			{
				_mesh[i].Dispose();
			}
		}
	}
}