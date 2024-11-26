using System;
using OpenTK.Mathematics;
using ContentPipeline;
using OpenTK.Graphics.OpenGL;
using CSGL.Assets;

namespace CSGL.Engine
{
	public class Light : Entity
	{
		public Color4 Colour = Color4.White;
		public float intensity = 1.0f;

		public Vector3 ambient = Vector3.One * 1f;
		public Vector3 diffuse = Vector3.One * 1f;
		public Vector3 specular = Vector3.One * 1f;

		public Light(Color4 colour, float ambient, float intensity, float diffuse, float specular) : base("Light")
		{
			this.Colour = colour;
			this.intensity = intensity;

			this.ambient = Vector3.One * ambient;
			this.diffuse = Vector3.One * diffuse;
			this.specular = Vector3.One * specular;

			Model model = Import.Model("cube.obj");

			Shader defaultShader = ShaderManager.Shaders["light.shader"];
			List<Texture> texList = new List<Texture>();

			Texture tex = new Texture("default.png", TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.UnsignedByte);

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