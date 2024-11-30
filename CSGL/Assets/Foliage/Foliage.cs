using ContentPipeline;
using CSGL.Engine;
using Logging;
using OpenTK.Graphics.OpenGL;
using SharedLibrary;
using CSGL.Graphics;
using OpenTK.Mathematics;

namespace CSGL.Assets
{
	public class Foliage : GameObject
	{
		public Foliage(Vector3 position) : base("Grass")
		{
			//Model model = ModelImporter.Import(Manifest.GetAsset<ModelAsset>("nodesnodes.obj").FilePath);

			this.transform.position = position;

			//this.AddTexture(new Texture("bush_diffuse.png", TextureType.DIFFUSE, TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.UnsignedByte));
			//this.AddTexture(new Texture("bush_specular.png", TextureType.SPECULAR, TextureTarget.Texture2D, 1, PixelFormat.Red, PixelType.UnsignedByte));

			this.model = new Model(Manifest.GetAsset<ModelAsset>("Bush.fbx"), ShaderManager.Shaders["grass.shader"]);

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

			//this.model._mesh[1].transform.rotation = Quaternion.FromEulerAngles(new Vector3(MathHelper.DegreesToRadians(delta), 0, MathHelper.DegreesToRadians(-delta))) * Time.deltaTime;
			//this.model._mesh[2].transform.rotation = Quaternion.FromEulerAngles(new Vector3(-MathHelper.DegreesToRadians(delta), 0, MathHelper.DegreesToRadians(-delta))) * Time.deltaTime;

			//this.
			base.Update();
		}

		public override void Render()
		{
			base.Render();
		}
	}
}
