using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CSGL.Graphics
{
	public class TextureParameter
	{
		public TextureTarget TextureTarget { get; set; }
		public TextureParameterName TextureParameterName {  get; set; }
		public int targetEnum { get; set; }
		public TextureParameter(TextureTarget textureTarget, TextureParameterName parameterName, int inEnum)
		{
			this.TextureTarget = textureTarget;
			this.TextureParameterName = parameterName;
			this.targetEnum = inEnum;
		}
	}
}
