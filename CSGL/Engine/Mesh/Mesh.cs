using ContentPipeline;
using ContentPipeline.Components;
using CSGL.Engine.OpenGL;
using OpenTK.Graphics.OpenGL;
using Logging;

namespace CSGL.Engine
{
	public class Mesh : Component, IDisposable
	{
		private List<Texture> textures = new List<Texture>();
		public Shader Shader = null!;

		public VAO VAO = null!;
		public VBO VBO = null!;
		public EBO EBO = null!;

		float[] vertexBuffer = null!;
		uint[] indexBuffer = null!;

		BufferUsageHint hint;
		public Mesh() { }

		public Mesh(Vertex[] vertices, uint[] indices, List<Texture> textures, Shader shader, BufferUsageHint hint = BufferUsageHint.StaticDraw)
		{
			//this.vbo = new VBO(vertices);

			this.vertexBuffer = MeshData.Buffer(vertices);
			this.indexBuffer = indices;
			this.Shader = shader;

			this.hint = hint;
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

		public void Dispose()
		{
			VAO.Dispose();
			VBO.Dispose();
			EBO.Dispose();
			Shader.Dispose();
			foreach (Texture tex in textures) 
			{
				tex.Dispose();
			}
		}

		public static Mesh FromModel(Model model, List<Texture> Textures, Shader shader)
		{
			Mesh mesh = new Mesh(model.Meshes[0].Vertices, model.Meshes[0].Indices, Textures, shader);

			return mesh;
		}

		public override string ToString()
		{
			string output = "VertexBuffer\n\tx\ty\tz\tx\ty\tz\tx\ty\tz\tu\tv";

			for (int i = 0; i < this.vertexBuffer.Length; i++)
			{
				if (i % Vertex.Stride == 0)
					output += "\n";

				output += "\t" + this.vertexBuffer[i];
			}

			output += "\nIndex Buffer\n";

			for (int i = 0; i < this.indexBuffer.Length; i++)
			{
				if (i % 3 == 0)
					output += "\n";
				output += "\t" + this.indexBuffer[i];
			}

			return output;
		}
	}
}