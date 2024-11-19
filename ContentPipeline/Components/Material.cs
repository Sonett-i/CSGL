using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentPipeline.Components
{
	public class Material
	{
		public string Name { get; set; }

		public Texture2D DiffuseTexture {  get; set; }
		public Texture2D NormalTexture { get; set; }

		public Material(string name, Texture2D diffuseTexture, Texture2D normalTexture)
		{
			this.Name = name;
			this.DiffuseTexture = diffuseTexture;
			this.NormalTexture = normalTexture;
		}
	}
}
