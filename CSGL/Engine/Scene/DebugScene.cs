using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logging;
using ContentPipeline;

namespace CSGL
{
	public class DebugScene : Scene
	{
		public DebugScene(string name) : base(name) 
		{

		}

		public override void Awake()
		{
			Log.Info($"{base.Name}({base.sceneID}) Scene Awake");

			Model? test = Manifest.GetAsset<Model>("cube.obj");

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
	}
}
