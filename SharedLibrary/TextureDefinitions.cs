using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SharedLibrary
{
	public enum TextureType
	{
		DIFFUSE,
		SPECULAR,
		NORMAL,
		HEIGHT
	}

	public enum TextureParameters
	{
		WrapMode
	}

	public class TextureDefinitions
	{
		public static Dictionary<TextureType, string> TextureUniformTypes = new Dictionary<TextureType, string>()
		{
			[SharedLibrary.TextureType.DIFFUSE] = "diffuse",
			[SharedLibrary.TextureType.SPECULAR] = "specular",
			[SharedLibrary.TextureType.NORMAL] = "normal",

		};

		public static Dictionary<string, TextureType> TextureType = new Dictionary<string, TextureType>()
		{
			["diffuse"] = SharedLibrary.TextureType.DIFFUSE,
			["specular"] = SharedLibrary.TextureType.SPECULAR,
			["normal"] = SharedLibrary.TextureType.NORMAL,
			["height"] = SharedLibrary.TextureType.HEIGHT
		};

		public static PixelInternalFormat ConvertPixelFormat(int components)
		{
			return components switch
			{
				0 => PixelInternalFormat.Rgba,
				1 => PixelInternalFormat.Luminance,
				2 => PixelInternalFormat.LuminanceAlpha,
				3 => PixelInternalFormat.Rgb,
				4 => PixelInternalFormat.Rgba,
				_ => throw new ArgumentOutOfRangeException(nameof(components), "Unknown colour component")
			};
		}

		public static PixelInternalFormat ConvertInternalFormat(int components)
		{
			return components switch
			{
				0 => PixelInternalFormat.Rgba,
				1 => PixelInternalFormat.Luminance,
				_ => throw new ArgumentOutOfRangeException(nameof(components), "unknown texture type")
			};
		}

		public static PixelFormat GetPixelFormat(TextureType type)
		{
			return type switch
			{ 
				SharedLibrary.TextureType.DIFFUSE => PixelFormat.Rgba,
				SharedLibrary.TextureType.SPECULAR => PixelFormat.Red,
				SharedLibrary.TextureType.NORMAL => PixelFormat.Rgb,
				SharedLibrary.TextureType.HEIGHT => PixelFormat.Luminance,
				_ => throw new ArgumentOutOfRangeException(nameof(type), "unknown texture type")
			};
		}
	}
}
