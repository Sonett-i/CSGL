using ContentPipeline;
using OpenTK.Graphics.OpenGL;
using SharedLibrary;
using StbImageSharp;
using System;
using System.IO;

namespace CSGL.Engine
{
	public class Texture
	{
		public int ID { get; private set; }
		public int unit = 0;
		public TextureTarget TextureTargetType { get; private set; }
		public TextureType TextureType { get; private set; }

		public Texture(string imagePath, TextureType textureType, TextureTarget textureTarget, int slot, PixelFormat format, PixelType pixelType)
		{
			this.TextureTargetType = textureTarget;
			this.TextureType = textureType;

			this.unit = slot;
			TextureAsset texAsset = Manifest.GetAsset<TextureAsset>(imagePath);

			// Load the image
			StbImage.stbi_set_flip_vertically_on_load(texAsset.isFlipped);

			using (FileStream stream = File.OpenRead(texAsset.FilePath))
			{
				ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

				// Generate OpenGL texture object
				ID = GL.GenTexture();
				GL.ActiveTexture(TextureUnit.Texture0 + slot);
				GL.BindTexture(textureTarget, ID);

				// Configure texture parameters
				GL.TexParameter(textureTarget, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.NearestMipmapLinear);
				GL.TexParameter(textureTarget, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

				GL.TexParameter(textureTarget, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
				GL.TexParameter(textureTarget, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

				// Upload the image to OpenGL
				GL.TexImage2D(textureTarget, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, format, pixelType, image.Data);
				GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

				// Unbind the texture
				GL.BindTexture(textureTarget, 0);
			}
			StbImage.stbi_set_flip_vertically_on_load(texAsset.isFlipped);
		}

		public void TexUnit(int shaderProgram, string uniform, int unit)
		{
			int location = GL.GetUniformLocation(shaderProgram, uniform);
			GL.UseProgram(shaderProgram);
			GL.Uniform1(location, unit);
		}

		public void TexUnit(Shader shaderProgram, string uniform, int unit)
		{
			TexUnit(shaderProgram.ID, uniform, unit);
		}

		public void Bind()
		{
			GL.ActiveTexture(TextureUnit.Texture0 + unit);
			GL.BindTexture(TextureTargetType, ID);
		}

		public void Unbind()
		{
			GL.BindTexture(TextureTargetType, 0);
		}

		public void Dispose()
		{
			GL.DeleteTexture(ID);
		}
	}

}
