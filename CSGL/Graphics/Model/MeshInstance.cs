using ContentPipeline;
using CSGL.Engine;
using OpenTK.Graphics.OpenGL;
using Logging;
using SharedLibrary;
using OpenTK.Mathematics;
using CSGL.Engine;
using Assimp.Unmanaged;

namespace CSGL.Graphics
{
	public class Instance : IDisposable
	{
		public string Name { get; set; }
		public VAO VAO;
		public VBO VBO;
		public VBO IBO;
		public EBO EBO;
		public Shader shader;

		bool initialized = false;
		public int instances;
		public Instance(VAO vao, VBO vbo, EBO ebo, Shader shader, List<Matrix4> transforms = null!) 
		{
			this.shader = shader;

			this.VAO = vao;
			this.VBO = vbo;
			this.EBO = ebo;

			this.instances = transforms.Count;
			this.IBO = new VBO(transforms);

			GL.BindVertexArray(VAO.ID);

			VAO.LinkAttrib(IBO, 4, 4, VertexAttribPointerType.Float, 16, 0);
			VAO.LinkAttrib(IBO, 5, 4, VertexAttribPointerType.Float, 16, 1 * 4);
			VAO.LinkAttrib(IBO, 6, 4, VertexAttribPointerType.Float, 16, 2 * 4);
			VAO.LinkAttrib(IBO, 7, 4, VertexAttribPointerType.Float, 16, 3 * 4);

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
			shader.Activate();

			VAO.Bind();
			EBO.Bind();

			shader.SetUniform("camPos", Camera.main.transform.position);

			//shader.SetUniform("model", Matrix4.Identity);
			shader.SetUniform("view", Camera.main.ViewMatrix);
			shader.SetUniform("projection", Camera.main.ProjectionMatrix);
			shader.SetUniform("nearClip", Camera.main.NearClip);
			shader.SetUniform("farClip", Camera.main.FarClip);

			//GL.DrawElements(PrimitiveType.Triangles, this.ebo.indexLength, DrawElementsType.UnsignedInt, 0);
			GL.DrawElementsInstanced(PrimitiveType.Triangles, this.EBO.indexLength, DrawElementsType.UnsignedInt, 0, instances);

			ErrorCode errorCode = GL.GetError();

			if (errorCode != ErrorCode.NoError)
			{
				Log.GL($"Error: {errorCode}");
			}

		}
	}
}