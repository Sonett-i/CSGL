using System;
using ContentPipeline;
using CSGL.Graphics;

namespace CSGL.Engine
{
	// Bridge between Engine and ContentPipeline
	public class Import
	{
		public static ModelAsset Model(string modelName)
		{
			return Manifest.GetAsset<ModelAsset>("cube.obj");
		}
	}
}
