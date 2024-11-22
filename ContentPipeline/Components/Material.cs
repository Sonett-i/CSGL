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

		public TextureAsset DiffuseTexture {  get; set; }
		public TextureAsset NormalTexture { get; set; }

		public Material(string name, TextureAsset diffuseTexture, TextureAsset normalTexture)
		{
			this.Name = name;
			this.DiffuseTexture = diffuseTexture;
			this.NormalTexture = normalTexture;
		}
	}
}
