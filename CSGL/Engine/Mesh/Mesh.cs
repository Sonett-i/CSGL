using ContentPipeline;
using ContentPipeline.Components;
using CSGL.Engine.OpenGL;
using OpenTK.Graphics.OpenGL;
using Logging;

namespace CSGL.Engine
{
	public class Mesh
	{
		private List<Texture2D> _texture = new List<Texture2D>();
		private Shader _shader;

		public VAO VAO;
		public VBO VBO;
		public EBO EBO;

		float[] vertexBuffer;
		uint[] indexBuffer;
		Shader shader;

		public Mesh(Vertex[] vertices, uint[] indices, List<Texture2D> textures, Shader shader)
		{
			this.shader = shader;
			BufferUsageHint hint = BufferUsageHint.StaticDraw;

			this.VAO = new VAO();
			this.VBO = new VBO(vertices, hint);
			this.EBO = new EBO(indices, hint);

			this.VAO.BindVBO(VBO);
		}

		public void Draw()
		{
			shader.Activate();

			this.VAO.Bind();
			this.EBO.Bind();

			GL.DrawElements(PrimitiveType.Triangles, EBO.Indices.Length, DrawElementsType.UnsignedInt, 0);

			ErrorCode error = GL.GetError();

			if (error != ErrorCode.NoError)
			{
				Log.GL($"Error drawing {this.ToString()}");
			}

		}
	}
}
