using ContentPipeline;
using CSGL.Engine;
using OpenTK.Graphics.OpenGL;
using Logging;
using SharedLibrary;
using OpenTK.Mathematics;
using System.Security.Cryptography.X509Certificates;

namespace CSGL.Graphics
{
	public class MeshNode
	{
		public List<Mesh> Meshes { get; set; } = new List<Mesh>();
		public List<MeshNode> Children { get; set; } = new List<MeshNode>();

		public MeshNode Parent { get; set; }

		public string Name { get; set; }
		public MeshNode(string name)
		{
			this.Name = name;
		}
	}

	public class Mesh : IDisposable
	{
		bool initialized = false;
		public string Name { get; set; }

		private List<Texture> textures = new List<Texture>();

		public VAO VAO = null!;
		public VBO VBO = null!;
		public EBO EBO = null!;

		float[] vertexBuffer = null!;
		uint[] indexBuffer = null!;

		public Mesh parent;

		public Transform transform = new Transform();

		public Matrix4 Transform_M = Matrix4.Identity;

		BufferUsageHint hint;
		public Mesh() { }

		public Mesh(string name)
		{
			Name = name;
		}

		public Mesh(Vertex[] vertices, uint[] indices, List<Texture> textures, string name, BufferUsageHint hint = BufferUsageHint.StaticDraw)
		{

			this.Name = name;

			this.vertexBuffer = Vertex.ToBuffer(vertices);
			this.indexBuffer = indices;

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
			initialized = true;
		}

		public void Draw(Shader shader, Camera camera)
		{
			if (this.VAO == null || this.EBO == null || this.VBO == null)
				return;

			shader.Activate();
			VAO.Bind();

			EBO.Bind();
			for (int i = 0; i < textures.Count; i++)
			{
				string type = TextureDefinitions.TextureUniformTypes[textures[i].TextureType];

				textures[i].TexUnit(shader, ("material." + type), i);
				textures[i].Bind();
			}

			shader.SetUniform("light.colour", SceneManager.ActiveScene.MainLight.Colour);

			shader.SetUniform("camPos", Camera.main.transform.position);

			shader.SetUniform("model", this.transform.Transform_Matrix);
			shader.SetUniform("view", camera.ViewMatrix);
			shader.SetUniform("projection", camera.ProjectionMatrix);
			shader.SetUniform("nearClip", camera.NearClip);
			shader.SetUniform("farClip", camera.FarClip);

			GL.DrawElements(PrimitiveType.Triangles, this.EBO.indexLength, DrawElementsType.UnsignedInt, 0);

			ErrorCode error = GL.GetError();

			if (error != ErrorCode.NoError)
			{
				Log.GL($"Error drawing {this.ToString()}");
			}
		}

		public void Dispose()
		{
			if (initialized == false)
				return;

			VAO.Dispose();
			VBO.Dispose();
			EBO.Dispose();
			//Shader.Dispose();
			foreach (Texture tex in textures) 
			{
				tex.Dispose();
			}
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