using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL
{
	public class Quad : RenderObject
	{
		public Quad(float[] vertices, uint[] indices, ShaderProgram shaderProgram) : base(vertices, indices, shaderProgram)
		{

		}
	}
}
