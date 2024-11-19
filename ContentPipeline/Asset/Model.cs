using ContentPipeline.Extensions;
using System;
using OpenTK.Mathematics;
using ContentPipeline.Components;


namespace ContentPipeline
{
	internal class Model : Asset
	{
		public int Vertices = 0;
		public int SubMeshes = 0;
		public Mesh[] Meshes = null!;

		public Model() 
		{
			base.Type = AssetType.ASSET_MODEL;
		}

		public Model(string filePath)
		{
			base.Type = AssetType.ASSET_MODEL;
		}

		public void InitializeFields(string filePath)
		{
			if (filePath == "")
				return;

			this.FilePath = filePath;
			this.Name = Path.GetFileName(filePath);
			this.ext = Path.GetExtension(filePath);

			switch (this.ext)
			{
				case ".obj":
					Obj.Import(this.FilePath);	
					break;
				case ".fbx":
					break;
			}
		}
	}
}
