using CSGL.Engine;
using OpenTK.Graphics.OpenGL;
using Logging;
using SharedLibrary;
using OpenTK.Mathematics;

namespace CSGL.Graphics
{
	public class Instance : IDisposable
	{
		public string? Name { get; set; }
		public VAO VAO;
		public VBO VBO;
		public VBO IBO;
		public EBO EBO;
		public Shader shader;

		public List<Texture> TextureList = new List<Texture>();

		bool initialized = false;
		public int instances;
		public Instance(VAO vao, VBO vbo, EBO ebo, Shader shader, List<Matrix4> transforms = null!, List<Texture> textures = null!) 
		{
			this.shader = shader;

			this.TextureList = textures;

			this.VAO = vao;
			this.VBO = vbo;
			this.EBO = ebo;

			this.instances = transforms.Count;
			this.IBO = new VBO(transforms);

			GL.BindVertexArray(VAO.ID);

			// Link each matrix4's vector4 to the VAO
			VAO.LinkAttrib(IBO, 4, 4, VertexAttribPointerType.Float, 16, 0);
			VAO.LinkAttrib(IBO, 5, 4, VertexAttribPointerType.Float, 16, 1 * 4);
			VAO.LinkAttrib(IBO, 6, 4, VertexAttribPointerType.Float, 16, 2 * 4);
			VAO.LinkAttrib(IBO, 7, 4, VertexAttribPointerType.Float, 16, 3 * 4);

			// Ensures that the linkedattribute
			GL.VertexAttribDivisor(4, 1);
			GL.VertexAttribDivisor(5, 1);
			GL.VertexAttribDivisor(6, 1);
			GL.VertexAttribDivisor(7, 1);

			ErrorCode error = GL.GetError();

			Log.GL(error.ToString());

			VAO.Unbind();
			VBO.Unbind();
			IBO.Unbind();
			EBO.Unbind();

			initialized = true;
		}


		public void Dispose()
		{
			if (initialized == false)
				return;
			Log.GL("Disposing: " + this.ToString());

			VAO.Dispose();
			VBO.Dispose();
			EBO.Dispose();

		}

		public override string ToString()
		{
			string output = $"{this.Name}";

			return output;
		}

		public void Draw()
		{
			GL.Disable(EnableCap.CullFace);
			shader.Activate();

			VAO.Bind();
			EBO.Bind();

			for (int i = 0; i < TextureList.Count; i++)
			{
				string type = TextureDefinitions.TextureUniformTypes[TextureList[i].TextureType];

				TextureList[i].TexUnit(shader, ("material." + type), i);
				TextureList[i].Bind();
			}

			shader.SetUniform("camPos", Camera.main.transform.position);

			shader.SetUniform("light.colour", SceneManager.ActiveScene.MainLight.Colour);
			shader.SetUniform("light.position", SceneManager.ActiveScene.MainLight.transform.position);

			shader.SetUniform("view", Camera.main.ViewMatrix);
			shader.SetUniform("projection", Camera.main.ProjectionMatrix);
			shader.SetUniform("nearClip", Camera.main.NearClip);
			shader.SetUniform("farClip", Camera.main.FarClip);

			GL.DrawElementsInstanced(PrimitiveType.Triangles, this.EBO.indexLength, DrawElementsType.UnsignedInt, 0, instances);

			ErrorCode errorCode = GL.GetError();

			if (errorCode != ErrorCode.NoError)
			{
				Log.GL($"Error: {errorCode}");
			}
			GL.Enable(EnableCap.CullFace);
		}
	}
}