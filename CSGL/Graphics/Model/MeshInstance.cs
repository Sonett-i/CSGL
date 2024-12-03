using ContentPipeline;
using CSGL.Engine;
using OpenTK.Graphics.OpenGL;
using Logging;
using SharedLibrary;
using OpenTK.Mathematics;

namespace CSGL.Graphics
{
	public class Instance : Mesh, IDisposable
	{
		public bool initialized = false;
		public string Name { get; set; }

		private List<Texture> textures = new List<Texture>();

		public int instanceCount;
		public List<Matrix4> instanceMatrix = new List<Matrix4>();

		public VAO VAO = null!;
		public VBO VBO = null!;
		public EBO EBO = null!;

		public VBO InstanceVBO = null!;

		float[] vertexBuffer = null!;
		uint[] indexBuffer = null!;

		BufferUsageHint hint;
		public Instance() { }

		public Instance(string name)
		{
			Name = name;
		}

		public Instance(Vertex[] vertices, uint[] indices, List<Texture> textures, string name, int instanceCount, List<Matrix4> transforms, BufferUsageHint hint = BufferUsageHint.StaticDraw)
		{
			this.Name = name;

			this.vertexBuffer = Vertex.ToBuffer(vertices);
			this.indexBuffer = indices;

			this.hint = hint;
			this.textures = textures;
			this.instanceCount = instanceCount;

			this.VBO = new VBO(vertices);
			this.InstanceVBO = new VBO(transforms);

			this.VAO = new VAO();

			VAO.Bind();

			this.VAO.LinkAttrib(VBO, 0, 3, VertexAttribPointerType.Float, Vertex.Stride, Vertex.PositionOffset);
			this.VAO.LinkAttrib(VBO, 1, 3, VertexAttribPointerType.Float, Vertex.Stride, Vertex.NormalOffset);
			this.VAO.LinkAttrib(VBO, 2, 3, VertexAttribPointerType.Float, Vertex.Stride, Vertex.TangentOffset);
			this.VAO.LinkAttrib(VBO, 3, 2, VertexAttribPointerType.Float, Vertex.Stride, Vertex.UVOffset);

			if (instancing != 1)
			{
				InstanceVBO.Bind();
				//VAO.LinkAttrib(InstanceVBO, 4, 4, VertexAttribPointerType.Float, 16)
			}

			ErrorCode error = GL.GetError();
			if (error != ErrorCode.NoError)
			{
				Log.GL($"Error drawing {this.ToString()}");
			}

			this.EBO = new EBO(indices);


			VAO.Unbind();
			initialized = true;
		}

		public void Draw(Shader shader, Camera camera, Matrix4 transformMatrix)
		{
			if (this.VAO == null || this.EBO == null || this.VBO == null)
				return;


			if (shader != null)
				shader.Activate();
			else
				ShaderManager.Shaders["default.shader"].Activate();

			VAO.Bind();

			EBO.Bind();
			for (int i = 0; i < textures.Count; i++)
			{
				string type = TextureDefinitions.TextureUniformTypes[textures[i].TextureType];

				textures[i].TexUnit(shader, ("material." + type), i);
				textures[i].Bind();
			}

			shader.SetUniform("light.colour", SceneManager.ActiveScene.MainLight.Colour);
			shader.SetUniform("light.position", SceneManager.ActiveScene.MainLight.transform.position);


			shader.SetUniform("camPos", Camera.main.transform.position);

			shader.SetUniform("model", transformMatrix);
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
			Log.GL("Disposing: " + this.ToString());

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
			string output = $"{this.Name}";

			return output;
		}
	}
}