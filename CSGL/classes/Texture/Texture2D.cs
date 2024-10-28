using System;
using StbImageSharp;
using OpenTK.Graphics.OpenGL;

namespace CSGL
{
	internal class Texture2D
	{
		public int TextureHandle;
		public string FilePath;
		public int Width; public int Height;

		public int channel;

		public Texture2D(string filePath)
		{
			FilePath = filePath;
			Width = 0; Height = 0;

			this.TextureHandle = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, this.TextureHandle);

			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
		}
	}
}
