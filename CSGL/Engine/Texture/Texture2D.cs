using System;
using StbImageSharp;
using OpenTK.Graphics.OpenGL;

namespace CSGL
{
	public class Texture2D
	{
		public static Texture2D? DefaultTexture;
		public static Texture2D? BadTexture;

		public ImageResult image;
		public int TextureHandle;
		public string Name;

		public int Width; 
		public int Height;

		public int channel;
		public byte[] data;

		public Texture2D(string filePath, string fileName, int channel = 0, bool flipped = false)
		{
			Name = fileName;

			// Generally we want to flip textures as coordinates are upside-down by default
			if (filePath.Contains("Skybox", StringComparison.OrdinalIgnoreCase))
			{
				StbImage.stbi_set_flip_vertically_on_load(0);
			}
			else
			{
				if (flipped)
					StbImage.stbi_set_flip_vertically_on_load(1);
			}
				

			// Bind active texture channel
			GL.ActiveTexture(TextureUnit.Texture0);

			this.TextureHandle = GL.GenTexture(); // Get a handle for this texture and bind as Texture2D
			GL.BindTexture(TextureTarget.Texture2D, this.TextureHandle);

			// Load the image file as filestream, load using Stbimage library
			using (FileStream stream = File.OpenRead(filePath))
			{
				ImageResult result = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
				this.image = result;
				this.Width = result.Width;
				this.Height = result.Height;
				this.data = result.Data;
				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, result.Width, result.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, result.Data);
			}

			StbImage.stbi_set_flip_vertically_on_load(0); // Unset the flipped parameter

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

			// Generate mipmaps
			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

			ErrorCode error = GL.GetError();

			if (error != ErrorCode.NoError)
			{
				Log.Error($"Error importing texture {filePath}: {error}");
			}
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

		// Remove resources from GPU
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
