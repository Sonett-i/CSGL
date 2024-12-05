using ContentPipeline;
using Logging;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL;
using SharedLibrary;
using StbImageSharp;
using System;
using System.IO;

namespace CSGL.Graphics
{
	public class Texture
	{
		public int ID { get; private set; }
		public int unit = 0;
		public TextureTarget TextureTargetType { get; private set; }
		public TextureType TextureType { get; private set; }

		public Texture() { } // Empty

		public Texture(string imagePath, TextureType textureType, TextureTarget textureTarget, int slot, PixelFormat format, PixelType pixelType, int flipped = -1)
		{
			this.TextureTargetType = textureTarget;
			this.TextureType = textureType;

			this.unit = slot;
			TextureAsset texAsset = Manifest.GetAsset<TextureAsset>(imagePath);

			int imageFlipped = (flipped != -1) ? flipped : texAsset.isFlipped;
			// Load the image
			StbImage.stbi_set_flip_vertically_on_load(imageFlipped);

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
			StbImage.stbi_set_flip_vertically_on_load(0);
		}

		public Texture(string path, string typeName, int wrapU, int wrapV, int unit)
		{
			this.TextureTargetType = TextureTarget.Texture2D;
			this.TextureType = TextureDefinitions.TextureType[typeName];
			this.unit = unit;

			

			StbImage.stbi_set_flip_vertically_on_load(1);

			using (FileStream stream = File.OpenRead(path))
			{
				ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

				// Generate OpenGL texture object
				ID = GL.GenTexture();
				GL.ActiveTexture(TextureUnit.Texture0 + unit);
				GL.BindTexture(this.TextureTargetType, ID);

				//image.Comp = ColorComponents.RedGreenBlue;
				

				// Configure texture parameters
				GL.TexParameter(this.TextureTargetType, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.NearestMipmapLinear);
				GL.TexParameter(this.TextureTargetType, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

				GL.TexParameter(this.TextureTargetType, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
				GL.TexParameter(this.TextureTargetType, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);


				// Upload the image to OpenGL
				GL.TexImage2D(this.TextureTargetType, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, TextureDefinitions.GetPixelFormat(this.TextureType), PixelType.UnsignedByte, image.Data);
				GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

				ErrorCode error = GL.GetError();

				if (error != ErrorCode.NoError)
				{
					Log.GL("Error: " + error.ToString());
				}

				// Unbind the texture
				GL.BindTexture(this.TextureTargetType, 0);
			}
			StbImage.stbi_set_flip_vertically_on_load(0);
		}

		public void SetParameters(params TextureParameter[] parameters)
		{
			this.Bind();

			foreach (TextureParameter parameter in parameters)
			{
				GL.TexParameter(parameter.TextureTarget, parameter.TextureParameterName, (int)parameter.targetEnum);

				ErrorCode error = GL.GetError();

				if (error != ErrorCode.NoError)
				{
					Log.GL($"Error: {error}");
				}
			}
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
