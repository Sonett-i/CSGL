using System;
using OpenTK;
using OpenTK.Mathematics;

namespace CSGL
{
	public class Monobehaviour
	{
		public Transform Transform;
		public Model Model;

		
		public Monobehaviour(Transform transform, Model model, Texture2D? texture = null)
		{
			this.Transform = transform;
			this.Model = model;
		}

		public virtual void OnAwake()
		{
			Log.Default($"{Model.Name} awake at: {Transform.Position.ToString()}");
			//Model.m_Model = MathU.TRS(Transform);
		}

		public virtual void Start()
		{

		}

		public virtual void Update()
		{
			//Model.m_Model = MathU.TRS(Transform);
		}

		public virtual void FixedUpdate()
		{

		}

		public virtual void OnRender()
		{
			//Model.Render();
		}
	}
}
