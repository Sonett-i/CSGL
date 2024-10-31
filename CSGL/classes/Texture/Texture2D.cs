using System;
using StbImageSharp;
using OpenTK.Graphics.OpenGL;

namespace CSGL
{
	public class Texture2D
	{
		public int TextureHandle;
		public string Name;

		public int Width; 
		public int Height;

		public int channel;

		public Texture2D(string filePath, string fileName, int channel = 0)
		{
			Name = fileName;

			GL.ActiveTexture(TextureUnit.Texture0);

			this.TextureHandle = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, this.TextureHandle);

			using (FileStream stream = File.OpenRead(filePath))
			{
				ImageResult result = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
				this.Width = result.Width;
				this.Height = result.Height;

				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, result.Width, result.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, result.Data);
			}

			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
		}

		public void UseTexture()
		{
			GL.BindTexture(TextureTarget.Texture2D, this.TextureHandle);
		}

		~Texture2D()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			GL.DeleteTexture(TextureHandle);
			GC.SuppressFinalize(this);
		}

		public override string ToString()
		{
			return $"Handle: {this.TextureHandle} Width: {this.Width}, Height: {this.Height}";
		}
	}
}
