using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL.Engine
{
	public class Model
	{
		public List<Mesh> meshes = new List<Mesh>();

		public void Draw(Shader shader, Camera camera)
		{
			for (int i = 0; i < meshes.Count; i++)
			{
				meshes[i].Draw(shader, camera);
			}
		}
	}
}
