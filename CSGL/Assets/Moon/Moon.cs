using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL
{
	public class Moon : Monobehaviour
	{
		MeshFilter meshFilter;
		MeshRenderer meshRenderer;

		public Moon()
		{
			meshFilter = this.GetComponent<MeshFilter>();
			meshRenderer = this.GetComponent<MeshRenderer>();

			meshFilter.Set(Resources.MeshFilters["Moon.obj"]);

			this.meshRenderer.Create(meshFilter, Resources.Materials["Planet"]);

		}

		public override void OnAwake()
		{
			base.OnAwake();
		}

		public override void Start()
		{
			base.Start();
		}

		public override void Update()
		{
			this.Transform.UpdateTransforms();
			base.Update();
		}
	}
}
