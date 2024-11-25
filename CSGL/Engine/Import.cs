using System;
using ContentPipeline;

namespace CSGL.Engine
{
	// Bridge between Engine and ContentPipeline
	public class Import
	{
		public static Model Model(string modelName)
		{
			return Manifest.GetAsset<Model>("cube.obj");
		}

		public static Shader Shader(string shaderName)
		{

			return null;
		}

		public static Texture Texture(string textureName)
		{
			return null;
		}
	}
}
