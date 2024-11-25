using System;
using OpenTK.Mathematics;
using ContentPipeline;
using CSGL.Assets;

namespace CSGL.Engine
{
	public class Light : Entity
	{
		public Color4 Colour = Color4.White;
		public float intensity = 1.0f;

		public Light(Color4 colour, float intensity) : base("Light")
		{
			this.Colour = colour;
			this.intensity = intensity;

			Model model = Import.Model("cube.obj");

			//Shader defaultShader = ShaderManager.Shaders["light.shader"];
			//List<Texture> texList = new List<Texture>();
			//Texture tex = Texture.LoadFromAsset(Manifest.GetAsset<TextureAsset>("default.png"));

			//tex.texUnit(defaultShader, "tex0", 0);
			//texList.Add(tex);

			//this.mesh = Mesh.FromModel(model, texList, defaultShader);
		}
	}
}