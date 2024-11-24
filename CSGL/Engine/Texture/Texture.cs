using System;
using OpenTK.Graphics.OpenGL;
using Logging;
using ContentPipeline;

namespace CSGL.Engine
{
	public class Texture : IDisposable
	{
		public readonly int ID;
		public TextureTarget Type;
		public TextureUnit Slot;
		public PixelFormat PixelFormat;
		public PixelType PixelType;

		public Texture(int handle, TextureTarget type)
		{
			this.ID = handle;
			this.Type = type;
		}

		public void texUnit(Shader shader, string uniform, int unit)
		{
			int texUni = GL.GetUniformLocation(shader.ID, uniform);
			shader.Activate();
			GL.Uniform1(texUni, unit);
		}

		public void Bind(TextureUnit unit)
		{
			GL.ActiveTexture(unit);
			GL.BindTexture(TextureTarget.Texture2D, ID);
		}

		public void Unbind()
		{
			GL.BindTexture(Type, 0);
		}

		~Texture()
		{
			this.Dispose();
		}
		public void Dispose()
		{
			GL.DeleteTexture(this.ID);
			GC.SuppressFinalize(this);
		}

		public static Texture LoadFromAsset(TextureAsset textureAsset)
		{
			int handle = GL.GenTexture();

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, handle);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, textureAsset.Width, textureAsset.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, textureAsset.data);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

			return new Texture(handle, TextureTarget.Texture2D);
		}
	}
}
