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
using CSGL.Assets; 

namespace CSGL
{
	public class DebugScene : Scene
	{

		public DebugScene(string name) : base(name)
		{

		}

		void InitScene()
		{
			Box box = new Box();

			renderScene.Add(box);
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
			Camera.main.transform.position = new Vector3(0, 0, 10);
			base.Start();
		}

		float t = 0;
		public override void Update()
		{
			t += 1;


			base.Update();
		}

		public override void FixedUpdate()
		{		
			base.FixedUpdate();
		}

		public override void Render()
		{
			base.Render();
		}
	}
}