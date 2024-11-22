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

		/*
		public Mesh(Vertex[] vertices, uint[] indices, List<Texture2D> textures, Shader shader)
		{
			this.VAO = new VAO();
			this._shader = shader;

			VAO.Bind();

			VBO = new VBO(vertices);
			EBO = new EBO(indices);

			VAO.BindVBO(VBO);

			VAO.LinkAttrib(VBO, 0, 3, VertexAttribPointerType.Float, Vertex.Stride, Vertex.PositionOffset);
			VAO.LinkAttrib(VBO, 1, 3, VertexAttribPointerType.Float, Vertex.Stride, Vertex.NormalOffset);
			VAO.LinkAttrib(VBO, 2, 3, VertexAttribPointerType.Float, Vertex.Stride, Vertex.TangentOffset);
			VAO.LinkAttrib(VBO, 3, 2, VertexAttribPointerType.Float, Vertex.Stride, Vertex.UVOffset);

			VBO.Unbind();
			EBO.Unbind();
			VAO.Unbind();
		}
		*/

		public VAO VAO;
		public VBO VBO;
		public EBO EBO;

		public VBO vbo;

		float[] vertexBuffer;
		uint[] indexBuffer;
		Shader shader;

		public Mesh(Vertex[] vertices, uint[] indices, List<Texture2D> textures, Shader shader)
		{
			//this.vbo = new VBO(vertices);

			this.vertexBuffer = MeshData.Buffer(vertices);
			this.indexBuffer = indices;
			this.shader = shader;
			BufferUsageHint hint = BufferUsageHint.StaticDraw;

			this.VBO = new VBO(vertices);

			this.VAO = new VAO();
			this.VAO.BindVBO(VBO);

			this.EBO = new EBO(indices); // GL.GenBuffer();


			GL.BindVertexArray(0);
			
		}

		public void Draw()
		{
			shader.Activate();

			this.VAO.Bind();
			this.EBO.Bind();
			//GL.BindVertexArray(this.VAO.ID);

			GL.DrawElements(PrimitiveType.Triangles, this.EBO.indexLength, DrawElementsType.UnsignedInt, 0);

			ErrorCode error = GL.GetError();

			if (error != ErrorCode.NoError)
			{
				Log.GL($"Error drawing {this.ToString()}");
			}

		}
	}
}
