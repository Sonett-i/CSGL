using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL
{
	public class Triangle : RenderObject
	{
		public Triangle(float[] vertices, uint[] indices, ShaderProgram shaderProgram) : base(vertices, indices, shaderProgram)
		{

		}
	}
}
