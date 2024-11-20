using ContentPipeline.Extensions;
using ContentPipeline.Utilities;
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
		public Material[] Materials = null!;

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
					Meshes = Obj.Import(this.FilePath);

					break;
				case ".fbx":
					break;
			}

			if (this.Meshes != null)
				this.SubMeshes = this.Meshes.Length;
		}

		public override string ToString()
		{
			string output = "";
			float size = 0.0f;

			foreach (Mesh mesh in Meshes)
			{
				output += mesh.ToString() + "\n";
				size += mesh.Size;
			}

			output += "Size: " + Util.KiB(size);
			return output;
		}
	}
}
