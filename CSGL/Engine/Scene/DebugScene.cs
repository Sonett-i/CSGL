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

		public override void Awake()
		{
			Log.Info($"{base.Name}({base.sceneID}) Scene Awake");

			Model? test = Manifest.GetAsset<Model>("cube.obj");

			Shader defaultShader = ShaderManager.Shaders["default.shader"];

			Vertex[] vertices =
			{
				new Vertex(new Vector3(0.5f, 0.5f, 0), Vector3.Zero, Vector3.Zero, Vector2.Zero),
				new Vertex(new Vector3(0.5f, -0.5f, 0), Vector3.Zero, Vector3.Zero, Vector2.Zero),
				new Vertex(new Vector3(-0.5f, -0.5f, 0), Vector3.Zero, Vector3.Zero, Vector2.Zero),
				new Vertex(new Vector3(-0.5f, 0.5f, 0), Vector3.Zero, Vector3.Zero, Vector2.Zero),
			};

			uint[] indices =
			{
				0, 1, 3,
				1, 2, 3
			};

			Mesh mesh = new Mesh(test.Meshes[0].Vertices, test.Meshes[0].Indices, null, defaultShader);

			renderScene.Add(mesh);

			//Mesh mesh = new Mesh(test.m)

			base.Awake();
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
			foreach (Mesh mesh in renderScene)
			{
				mesh.Draw();
			}
			base.Render();
		}
	}
}
