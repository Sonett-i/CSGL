using ContentPipeline.Extensions;
using ContentPipeline.Utilities;
using System;
using OpenTK.Mathematics;
using ContentPipeline;


namespace ContentPipeline
{
	public class ModelAsset : Asset
	{
		public ModelAsset() 
		{
			base.Type = AssetType.ASSET_MODEL;
		}

		public ModelAsset(string filePath)
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
		}

		public override string ToString()
		{
			return $"{this.ID}: {this.Name}";
		}
	}
}
