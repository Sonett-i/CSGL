using System;
using OpenTK;
using OpenTK.Mathematics;

namespace CSGL
{
	public class Monobehaviour
	{
		public static List<Monobehaviour> Monobehaviours = new List<Monobehaviour>();

		public Transform Transform;
		public MeshRenderer MeshRenderer;

		public Monobehaviour(Transform transform, MeshRenderer meshRenderer)
		{
			this.Transform = transform;
			this.MeshRenderer = meshRenderer;

			Monobehaviours.Add(this);
		}

		public virtual void OnAwake()
		{
			Log.Default($"awake at: {Transform.Position.ToString()}");
			//RenderObject.m_Model = MathU.TRS(Transform);
		}

		public virtual void Start()
		{

		}

		public virtual void Update()
		{
			//MeshRenderer.m_model = MathU.TRS(Transform);
		}

		public virtual void FixedUpdate()
		{

		}

		public virtual void OnRender()
		{
			this.MeshRenderer.m_model = MathU.TRS(this.Transform);
			MeshRenderer.Render();
		}
	}
}
