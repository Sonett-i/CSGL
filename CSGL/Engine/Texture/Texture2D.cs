using System;
using StbImageSharp;
using OpenTK.Graphics.OpenGL;

namespace CSGL
{
	public class Texture2D
	{
		public static Texture2D? DefaultTexture;
		public static Texture2D? BadTexture;


		public int TextureHandle;
		public string Name;

		public int Width; 
		public int Height;

		public int channel;
		public byte[] data;

		public Texture2D(string filePath, string fileName, int channel = 0, bool flipped = false)
		{
			Name = fileName;

			GL.ActiveTexture(TextureUnit.Texture0);

			this.TextureHandle = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, this.TextureHandle);

			if (flipped)
				StbImage.stbi_set_flip_vertically_on_load(1);

			using (FileStream stream = File.OpenRead(filePath))
			{
				ImageResult result = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
				this.Width = result.Width;
				this.Height = result.Height;
				this.data = result.Data;
				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, result.Width, result.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, result.Data);
			}

			StbImage.stbi_set_flip_vertically_on_load(0);

			//GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			//GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

			//GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			//GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
		}

		public void UseTexture(TextureUnit unit)
		{
			GL.ActiveTexture(unit);
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
