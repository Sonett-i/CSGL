using ContentPipeline;
using CSGL.Engine;
using Logging;
using OpenTK.Mathematics;

namespace CSGL.Assets
{
	public class Box : Entity
	{
		public Box() : base("Box")
		{
			Model box = Manifest.GetAsset<Model>("plane.obj");
			Shader defaultShader = ShaderManager.Shaders["default.shader"];
			List<Texture> texList = new List<Texture>();
			Texture tex = Texture.LoadFromAsset(Manifest.GetAsset<TextureAsset>("planks.png"), 0);

			tex.texUnit(defaultShader, "tex0", 0);
			texList.Add(tex);

			Log.Info("Loading box mesh");

			this.mesh = Mesh.FromModel(box, texList, defaultShader);

			Log.GL(this.mesh.ToString());
		}

		float delta = 0;

		public override void Start()
		{
			this.transform.rotation = Quaternion.FromEulerAngles(0, 0, 0);
			this.transform.LocalScale = Vector3.One;
			this.transform.position = new Vector3(0, 0, 0);
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

			this.mesh.Shader.Uniforms["lightColor"].SetValue(SceneManager.ActiveScene.MainLight.Colour);
			this.mesh.Shader.Uniforms["lightPos"].SetValue(SceneManager.ActiveScene.MainLight.transform.position);
			//this.mesh.Shader.Uniforms["lightPos"].SetValue(Camera.main.transform.position);

			this.mesh.Shader.Uniforms["camPos"].SetValue(Camera.main.transform.position);

			base.Render();
		}
	}
}
