using ContentPipeline;
using ContentPipeline.Components;
using CSGL.Engine.OpenGL;
using OpenTK.Graphics.OpenGL;
using Logging;

namespace CSGL.Engine
{
	public class Mesh : Component
	{
		private List<Texture> textures = new List<Texture>();
		public Shader Shader;

		public VAO VAO;
		public VBO VBO;
		public EBO EBO;

		public VBO vbo;

		float[] vertexBuffer;
		uint[] indexBuffer;

		public Mesh() { }


		public Mesh(Vertex[] vertices, uint[] indices, List<Texture> textures, Shader shader)
		{
			//this.vbo = new VBO(vertices);

			this.vertexBuffer = MeshData.Buffer(vertices);
			this.indexBuffer = indices;
			this.Shader = shader;
			BufferUsageHint hint = BufferUsageHint.StaticDraw;
			this.textures = textures;

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

			Shader.Uniforms["model"].SetValue(this.ParentEntity.GetComponent<Transform>().Transform_Matrix);
			Shader.Uniforms["view"].SetValue(Camera.main.ViewMatrix);
			Shader.Uniforms["projection"].SetValue(Camera.main.ProjectionMatrix);

			this.VAO.Bind();
			this.EBO.Bind();

			for (int i = 0; i < this.textures.Count; i++)
			{
				GL.ActiveTexture(TextureUnit.Texture0 + i);
				GL.BindTexture(TextureTarget.Texture2D, textures[i].ID);
			}

			GL.DrawElements(PrimitiveType.Triangles, this.EBO.indexLength, DrawElementsType.UnsignedInt, 0);

			ErrorCode error = GL.GetError();

			if (error != ErrorCode.NoError)
			{
				Log.GL($"Error drawing {this.ToString()}");
			}
		}
	}
}