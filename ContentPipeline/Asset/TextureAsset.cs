using System;
using System.Collections.Generic;
using StbImageSharp;
using Logging;

namespace ContentPipeline
{
	public class TextureAsset : Asset
	{
		public int Width;
		public int Height;

		public bool isFlipped = false;
		public byte[] data = null!;


		public TextureAsset(string filePath)
		{
			base.Type = AssetType.ASSET_TEXTURE;
		}

		public void InitializeFields(string filePath)
		{
			if (filePath == "")
				return;

			int flip = 1;
			if (filePath.Contains("Skybox", StringComparison.OrdinalIgnoreCase))
			{
				flip = 0;
			}

			StbImage.stbi_set_flip_vertically_on_load(flip);

			this.Name = Path.GetFileName(filePath);
			this.ext = Path.GetExtension(filePath);

			using (FileStream stream = File.OpenRead(filePath))
			{
				ImageResult result = ImageResult.FromStream(stream);
				this.Width = result.Width;
				this.Height = result.Height;
				this.data = result.Data;
			}

			Log.Info($"{this.Name} (Width: {this.Width}, Height: {this.Height}) loaded {this.ID}");
			StbImage.stbi_set_flip_vertically_on_load(0);
		}
	}
}
