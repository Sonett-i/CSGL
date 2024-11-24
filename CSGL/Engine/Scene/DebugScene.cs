using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logging;
using ContentPipeline;
using CSGL.Engine;
using CSGL.Engine.Shaders;
using ContentPipeline.Components;
using OpenTK.Mathematics;

namespace CSGL
{
	public class DebugScene : Scene
	{
		public DebugScene(string name) : base(name)
		{

		}

		List<Mesh> renderScene = new List<Mesh>();
		Mesh mesh;

		void InitScene()
		{
			Model test = Manifest.GetAsset<Model>("cube.obj");

			Shader defaultShader = ShaderManager.Shaders["default.shader"];

			List<Texture> texList = new List<Texture>();

			Texture tex = Texture.LoadFromAsset(Manifest.GetAsset<TextureAsset>("wall.jpg"));
			tex.texUnit(defaultShader, "tex0", 0);

			texList.Add(tex);

			mesh = new Mesh(test.Meshes[0].Vertices, test.Meshes[0].Indices, texList, defaultShader);

			mesh.Shader.SetUniform("scale", 2f);

			renderScene.Add(mesh);
		}

		public override void Awake()
		{

			Log.Info($"{base.Name}({base.sceneID}) Scene Awake");
			InitScene();

			//Mesh mesh = new Mesh(test.m)

			base.Awake();
		}

		public override void Start()
		{
			base.Start();
		}

		float t = 0;
		public override void Update()
		{

			base.Update();
		}

		public override void Render()
		{
			foreach (Mesh mesh in renderScene)
			{
				mesh.Draw();
			}
			base.Render();
		}
	}
}