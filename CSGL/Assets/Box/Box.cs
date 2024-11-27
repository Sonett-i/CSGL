using ContentPipeline;
using CSGL.Engine;
using Logging;
using OpenTK.Graphics.OpenGL;
using SharedLibrary;

namespace CSGL.Assets
{
	public class Box : GameObject
	{
		public Box() : base("Box")
		{
			ModelAsset box = Manifest.GetAsset<ModelAsset>("plane.obj");
			Shader defaultShader = ShaderManager.Shaders["default.shader"];

			

			this.AddTexture(new Texture("planks.png", TextureType.DIFFUSE, TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.UnsignedByte));
			this.AddTexture(new Texture("planksSpec.png", TextureType.SPECULAR, TextureTarget.Texture2D, 1, PixelFormat.Red, PixelType.UnsignedByte));

			this.model = new Model(box, this.Textures);

			Log.Info("Loading box mesh");

			base.Lit = true;
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

			base.Render();
		}
	}
}
