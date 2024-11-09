using System;
using System.Numerics;

namespace CSGL
{
	public class GameObject : Monobehaviour
	{
		
		public string Name = "";


		public GameObject() : base()
		{

		}

		public override void Start()
		{
			base.Start();
		}

		public override void Update()
		{
			base.Update();
		}

		public override void OnRender()
		{
			this.GetComponent<MeshRenderer>().Render();
			base.OnRender();
		}
	}
}
