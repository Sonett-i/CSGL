using System;
using StbImageSharp;
using OpenTK.Graphics.OpenGL;
using ContentPipeline;
using Logging;

namespace CSGL.Engine
{
	internal class TextureManager
	{
		// OpenGL Config
		public static int MaxTextures;
		public static int MaxTextureSize;

		public static Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();

		public static Dictionary<string, int> PixelFormats = new Dictionary<string, int>()
		{
			[".png"] = (int) PixelFormat.Rgba,
			[".jpg"] = (int) PixelFormat.Rgb
		};
	}
}
