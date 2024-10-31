using System;
using OpenTK;
using OpenTK.Mathematics;

namespace CSGL
{
	public class Monobehaviour
	{
		public Transform Transform;
		public RenderObject RenderObject;
		public Texture2D? Texture;

		
		public Monobehaviour(Transform transform, RenderObject RenderObject, Texture2D? texture = null)
		{
			this.Transform = transform;
			this.RenderObject = RenderObject;

			if (texture != null)
			{
				Texture = texture;
				this.RenderObject.Material = MaterialManager.GetMaterial("textured");// ShaderManager.GetShader("textured");
			}
			else
			{
				this.RenderObject.Material = MaterialManager.GetMaterial("default");
			}

		}

		public virtual void OnAwake()
		{

		}

		public virtual void Start()
		{

		}

		public virtual void Update()
		{
			RenderObject.m_Model = MathU.TRS(Transform);
		}

		public virtual void FixedUpdate()
		{

		}

		public virtual void OnRender()
		{
			if (Texture != null)
			{
				Texture.UseTexture();
			}

			RenderObject.Render();
		}
	}
}
