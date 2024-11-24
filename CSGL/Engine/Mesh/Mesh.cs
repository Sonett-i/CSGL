using ContentPipeline;
using ContentPipeline.Components;
using CSGL.Engine.OpenGL;
using OpenTK.Graphics.OpenGL;
using Logging;

namespace CSGL.Engine
{
	public class Mesh
	{
		private List<Texture> _texture = new List<Texture>();
		public Shader Shader;

		public VAO VAO;
		public VBO VBO;
		public EBO EBO;

		public VBO vbo;

		float[] vertexBuffer;
		uint[] indexBuffer;


		public Mesh(Vertex[] vertices, uint[] indices, List<Texture> textures, Shader shader)
		{
			//this.vbo = new VBO(vertices);

			this.vertexBuffer = MeshData.Buffer(vertices);
			this.indexBuffer = indices;
			this.Shader = shader;
			BufferUsageHint hint = BufferUsageHint.StaticDraw;

			this.VBO = new VBO(vertices);

			this.VAO = new VAO();

			VAO.Bind();

			this.VAO.LinkAttrib(VBO, 0, 3, VertexAttribPointerType.Float, Vertex.Stride, Vertex.PositionOffset);
			this.VAO.LinkAttrib(VBO, 1, 3, VertexAttribPointerType.Float, Vertex.Stride, Vertex.NormalOffset);
			this.VAO.LinkAttrib(VBO, 2, 3, VertexAttribPointerType.Float, Vertex.Stride, Vertex.TangentOffset);
			this.VAO.LinkAttrib(VBO, 3, 2, VertexAttribPointerType.Float, Vertex.Stride, Vertex.UVOffset);


			ErrorCode error = GL.GetError();
			if (error != ErrorCode.NoError)
			{
				Log.GL($"Error drawing {this.ToString()}");
			}

			this.EBO = new EBO(indices);


			VAO.Unbind();
		}

		public void Draw()
		{
			Shader.Activate();

			this.VAO.Bind();
			this.EBO.Bind();

			GL.DrawElements(PrimitiveType.Triangles, this.EBO.indexLength, DrawElementsType.UnsignedInt, 0);

			ErrorCode error = GL.GetError();

			if (error != ErrorCode.NoError)
			{
				Log.GL($"Error drawing {this.ToString()}");
			}
		}
	}
}