using System;
using OpenTK.Mathematics;

namespace CSGL
{
	public class Windmill : Monobehaviour
	{
		public Vector3 spinAxis = Vector3.UnitX;
		float t = 0;

		GameObject propeller;
		Transform propellerRotation = new Transform(Vector3.Zero, Quaternion.Identity, Vector3.One);
		Vector3 propellerOffset = (-1.5f, 19.0f, 0.0f);

		float angle = 0;
		float rotationSpeed = 2.5f;

		public Windmill() : base()
		{
			Transform transform = new Transform();
			Transform prop = new Transform(transform.Position + propellerOffset * 0.5f, transform.Rotation, transform.Scale);
			//propeller = new GameObject(prop, new MeshRenderer(ModelManager.LoadModel("Propeller.obj"), MaterialManager.Default));
			
		}

		public override void Start()
		{
			this.rotationSpeed = CSGLU.Random(5, 7);
			//propeller.Transform.Position = new Vector3(0, -20, 0);
			//GameObject propeller = new GameObject()
			base.Start();
		}

		public override void Update()
		{
			angle += rotationSpeed * Time.deltaTime;
			//propeller.Transform.Rotation = Quaternion.FromAxisAngle(spinAxis.Normalized(), angle);
			base.Update();
		}

		public override void OnRender()
		{
			base.OnRender();
		}
	}
}
