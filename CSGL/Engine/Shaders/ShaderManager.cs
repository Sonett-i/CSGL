using System;
using ContentPipeline;
using Logging;

namespace CSGL.Engine.Shaders
{
	public class ShaderManager
	{
		public static Dictionary<string, Shader> Shaders = new Dictionary<string, Shader>();

		public static void CompileShaders()
		{
			List<ShaderAsset> shadersToCompile = new List<ShaderAsset>();

			foreach (KeyValuePair<Guid, Asset> kvp in Manifest.FileManifest[AssetType.ASSET_SHADER])
			{
				ShaderAsset shaderA = (ShaderAsset) kvp.Value;

				// Pass vertex/fragment shader file to shader class for compilation
				try
				{
					Shader shader = new Shader(shaderA.Name, shaderA.VertexShader, shaderA.FragmentShader);
					Shaders[shader.Name] = shader;
				}
				catch (Exception ex)
				{
					Log.GL($"Error: {ex.Message}");
				}
			}
		}
	}
}
