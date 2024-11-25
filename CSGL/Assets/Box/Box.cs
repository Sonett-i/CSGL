using ContentPipeline;
using CSGL.Engine;
using CSGL.Engine.Shaders;
using OpenTK.Mathematics;

namespace CSGL.Assets
{
	public class Box : Entity
	{
		public Box() : base("Box")
		{
			Model box = Manifest.GetAsset<Model>("cube.obj");

			Shader defaultShader = ShaderManager.Shaders["default.shader"];

			List<Texture> texList = new List<Texture>();

			Texture tex = Texture.LoadFromAsset(Manifest.GetAsset<TextureAsset>("default.png"));

			tex.texUnit(defaultShader, "tex0", 0);
			texList.Add(tex);

			this.mesh = Mesh.FromModel(box, texList, defaultShader);
		}

		float delta = 0;

		public override void Start()
		{
			this.transform.rotation = Quaternion.FromEulerAngles(1.51f, 0, 0);
			base.Start();
		}

		public override void Update()
		{
			delta++;

			this.transform.rotation = Quaternion.FromEulerAngles(new Vector3(MathHelper.DegreesToRadians(delta), 0, 0)) * Time.deltaTime;
			base.Update();
		}
	}
}
