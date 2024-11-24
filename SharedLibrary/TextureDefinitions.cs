using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
	public enum TextureType
	{
		DIFFUSE,
		SPECULAR,
		NORMAL
	}

	public class TextureDefinitions
	{
		public static Dictionary<string, TextureType> TextureType = new Dictionary<string, TextureType>()
		{
			["diffuse"] = SharedLibrary.TextureType.DIFFUSE,
			["specular"] = SharedLibrary.TextureType.SPECULAR,
			["normal"] = SharedLibrary.TextureType.NORMAL
		};
	}
}
