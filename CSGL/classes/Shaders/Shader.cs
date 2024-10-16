using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL
{
	public class Shader
	{
		public string Name;

		public Shader(string Name)
		{
			this.Name = Name; 
		}

	}

	public class VertexShader : Shader
	{
		public string ShaderCode;
		public int VertexShaderHandle;

		public VertexShader(string fileName, string shaderCode) : base(fileName)
		{
			this.ShaderCode = shaderCode;
		}
	}

	public class FragmentShader : Shader
	{
		public string ShaderCode;
		public int FragmentShaderHandle;

		public FragmentShader(string fileName, string shaderCode) : base(fileName)
		{
			this.ShaderCode = shaderCode;
		}
	}
}
