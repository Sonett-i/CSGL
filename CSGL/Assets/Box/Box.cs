using ContentPipeline;
using CSGL.Engine;
using Logging;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using StbImageSharp;

namespace CSGL.Assets
{
	public class Box : Entity
	{
		Texture diffuse;
		Texture specular;

		public Box() : base("Box")
		{
			Model box = Manifest.GetAsset<Model>("plane.obj");
			Shader defaultShader = ShaderManager.Shaders["default.shader"];
			List<Texture> texList = new List<Texture>();

			diffuse = new Texture("planks.png", TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.UnsignedByte);

			specular = new Texture("planksSpec.png", TextureTarget.Texture2D, 1, PixelFormat.Red, PixelType.UnsignedByte);
			//specular = new Texture("water_height.png", TextureTarget.Texture2D, 1, PixelFormat.Red, PixelType.UnsignedByte);

			Log.Info("Loading box mesh");

			//diffuse.TexUnit(defaultShader, "tex0", 0);
			//specular.TexUnit(defaultShader, "tex1", 1);

			//texList.Add(diffuse);
			//texList.Add(specular);

			this.mesh = Mesh.FromModel(box, texList, defaultShader);

			Log.GL(this.mesh.ToString());
		}

		float delta = 0;

		public override void Start()
		{
			base.Start();
		}

		public override void Update()
		{
			delta++;
			//this.transform.rotation = Quaternion.FromEulerAngles(new Vector3(MathHelper.DegreesToRadians(delta), 0, MathHelper.DegreesToRadians(-delta))) * Time.deltaTime;
			base.Update();
		}

		public override void Render()
		{
			this.mesh.Shader.Activate();
			this.mesh.Shader.Uniforms["model"].SetValue(this.transform.Transform_Matrix);
			this.mesh.Shader.Uniforms["view"].SetValue(Camera.main.ViewMatrix);
			this.mesh.Shader.Uniforms["projection"].SetValue(Camera.main.ProjectionMatrix);

			diffuse.Bind();
			this.mesh.Shader.SetUniform("material.diffuse", diffuse.unit);

			specular.Bind();
			this.mesh.Shader.SetUniform("material.specular", specular.unit);
			//this.mesh.Shader.SetUniform("material.shininess", 4.0f);

			this.mesh.Shader.SetUniform("light.position", SceneManager.ActiveScene.MainLight.transform.position);
			//this.mesh.Shader.SetUniform("light.ambient", SceneManager.ActiveScene.MainLight.ambient);
			//this.mesh.Shader.SetUniform("light.diffuse", SceneManager.ActiveScene.MainLight.diffuse);
			//this.mesh.Shader.SetUniform("light.specular", SceneManager.ActiveScene.MainLight.specular);
			this.mesh.Shader.SetUniform("light.colour", SceneManager.ActiveScene.MainLight.Colour);
			//this.mesh.Shader.SetUniform("light.a", 3f);
			//this.mesh.Shader.SetUniform("light.b", 0.7f);


			this.mesh.Shader.SetUniform("camPos", Camera.main.transform.position);


			base.Render();
		}
	}
}
