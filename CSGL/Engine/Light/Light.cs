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

			Shader defaultShader = ShaderManager.Shaders["light.shader"];
			List<Texture> texList = new List<Texture>();
			Texture tex = Texture.LoadFromAsset(Manifest.GetAsset<TextureAsset>("default.png"), 0);

			tex.texUnit(defaultShader, "tex0", 0);
			texList.Add(tex);

			this.mesh = Mesh.FromModel(model, texList, defaultShader);
		}

		public override void Start()
		{

			base.Start();
		}

		public override void Update()
		{

			base.Update();
		}

		public override void Render()
		{
			this.mesh.Shader.Activate();
			this.mesh.Shader.Uniforms["model"].SetValue(this.transform.Transform_Matrix);
			this.mesh.Shader.Uniforms["view"].SetValue(Camera.main.ViewMatrix);
			this.mesh.Shader.Uniforms["projection"].SetValue(Camera.main.ProjectionMatrix);

			this.mesh.Shader.Uniforms["lightColor"].SetValue(Colour);

			base.Render();
		}
	}
}