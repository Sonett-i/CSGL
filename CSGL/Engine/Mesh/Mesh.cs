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

		public int VAO;
		public int VBO;
		public int EBO;

		float[] vertexBuffer;
		uint[] indexBuffer;
		Shader shader;

		public Mesh(Vertex[] vertices, uint[] indices, List<Texture2D> textures, Shader shader)
		{
			this.vertexBuffer = MeshData.Buffer(vertices);
			this.indexBuffer = indices;
			this.shader = shader;
			BufferUsageHint hint = BufferUsageHint.StaticDraw;

			this.VBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.VBO);
			GL.BufferData(BufferTarget.ArrayBuffer, this.vertexBuffer.Length * sizeof(float), this.vertexBuffer, hint);

			this.VAO = GL.GenVertexArray();
			GL.BindVertexArray(this.VAO);
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.VBO);

			// Positional Data (vec3): Uniform Layout 0
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			// Vertex Normals (vec3): Uniform Layout 1
			GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 12 * sizeof(float), 3 * sizeof(float));
			GL.EnableVertexAttribArray(1);

			// Tangent Data (vec3): Uniform Layout 2
			GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 12 * sizeof(float), 6 * sizeof(float));
			GL.EnableVertexAttribArray(2);

			// TexCoord (vec2): Uniform Layout 3
			GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, 12 * sizeof(float), 9 * sizeof(float));
			GL.EnableVertexAttribArray(3);

			this.EBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.EBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indexBuffer.Length * sizeof(uint), indexBuffer, hint);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

			GL.BindVertexArray(0);
			
		}

		public void Draw()
		{
			shader.Activate();

			GL.BindVertexArray(this.VAO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.EBO);

			GL.DrawElements(PrimitiveType.Triangles, indexBuffer.Length, DrawElementsType.UnsignedInt, 0);

			ErrorCode error = GL.GetError();

			if (error != ErrorCode.NoError)
			{
				Log.GL($"Error drawing {this.ToString()}");
			}

		}
	}
}
