using System;
using System.Collections.Generic;
using StbImageSharp;
using Logging;
using SharedLibrary;

#pragma warning disable 8618

namespace ContentPipeline
{
	public class TextureAsset : Asset
	{
		public int Width;
		public int Height;

		public int isFlipped = 1;
		public TextureType TextureType;
		public byte[] data;
		

		public byte[] Load(int colourComponents)
		{
			using (FileStream stream = File.OpenRead(this.FilePath))
			{
				ImageResult result = ImageResult.FromStream(stream, (ColorComponents) colourComponents);
				return result.Data;
			}
		}

		public TextureAsset(string filePath)
		{
			base.Type = AssetType.ASSET_TEXTURE;
		}

		public void InitializeFields(string filePath)
		{
			if (filePath == "")
				return;

			TextureType texType = TextureType.DIFFUSE;

			this.isFlipped = 1;
			if (filePath.Contains("Skybox", StringComparison.OrdinalIgnoreCase))
			{
				this.isFlipped = 0;
			}

			if (filePath.Contains("diffuse"))
				texType = TextureType.DIFFUSE;

			this.FilePath = filePath;
			this.Name = Path.GetFileName(filePath);
			this.ext = Path.GetExtension(filePath);
			this.TextureType = texType;

			using (FileStream stream = File.OpenRead(filePath))
			{
				ImageResult result = ImageResult.FromStream(stream);
				this.Width = result.Width;
				this.Height = result.Height;
				this.data = result.Data;
			}

			Log.Info($"{this.Name} (Width: {this.Width}, Height: {this.Height}) loaded {this.ID}");
		}
	}
}
