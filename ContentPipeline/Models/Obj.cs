using System;
using ContentPipeline.Components;
using Assimp;

namespace ContentPipeline.Extensions
{
	public class Obj
	{
		public static void Import(string filePath)
		{
			AssimpContext context = new AssimpContext();

			Scene scene = context.ImportFile(filePath, PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals);

			List<Assimp.Mesh> meshes = scene.Meshes;
			List<Assimp.Material> materials = scene.Materials;


			//return meshes.ToArray();
		}
	}
}
