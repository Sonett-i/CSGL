using System;
using OpenTK;
using OpenTK.Mathematics;

namespace CSGL
{
	public class Monobehaviour
	{
		public Transform Transform;
		public RenderObject RenderObject;

		
		public Monobehaviour(Transform transform, RenderObject RenderObject)
		{
			this.Transform = transform;
			this.RenderObject = RenderObject;
		}

		public virtual void OnAwake()
		{

		}

		public virtual void Start()
		{

		}

		public virtual void Update()
		{
			RenderObject.m_Model = MathFuncs.TRS(Transform);
		}

		public virtual void FixedUpdate()
		{

		}
	}
}
