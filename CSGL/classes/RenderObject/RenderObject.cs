using System;
using System.Numerics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CSGL
{
	public class RenderObject : IDisposable
	{
		private bool disposed;
		private bool initialized;

		private int vbo; // Vertex Buffer Object
		private int vao; // Vertex Array Object
		private int ebo; // Element Buffer Object

		float[] vertices;
		uint[] indices;

		public string name;

		public ShaderProgram shaderProgram;
		BufferUsageHint hint;

		public RenderObject(float[] vertices, uint[] indices, ShaderProgram shaderProgram, BufferUsageHint hint = BufferUsageHint.StaticDraw)
		{
			this.vertices = vertices;
			this.indices = indices;
			this.hint = hint;

			this.name = "default";

			Log.Default("Bind Vertex Buffer Object");
			this.vbo = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbo);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, hint);

			Log.Default("Bind Vertex Array Object");
			this.vao = GL.GenVertexArray();
			GL.BindVertexArray(this.vao);
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbo);

			// Positional Data
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 7 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			// Vertex Colour
			GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 7 * sizeof(float), 3 * sizeof(float));
			GL.EnableVertexAttribArray(1);

			// Indices
			Log.Default("Bind Index Buffer Object");
			this.ebo = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ebo);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

			GL.BindVertexArray(0);
			this.shaderProgram = shaderProgram;
			
			initialized = true;
		}

		public RenderObject(Model model, ShaderProgram shaderProgram, BufferUsageHint hint = BufferUsageHint.StaticDraw)
		{
			this.hint = hint;
			this.name = model.name;

			// Vertices array stores position, color, and texture coordinates
			this.vertices = new float[model.vertices.Length * 9];
			int vertexIndex = 0;

			for (int i = 0; i < model.vertices.Length; i++)
			{
				// Vec3 Vertex Position
				this.vertices[vertexIndex] = model.vertices[i].x;
				this.vertices[vertexIndex + 1] = model.vertices[i].y;
				this.vertices[vertexIndex + 2] = model.vertices[i].z;

				// Vec4 Colour
				this.vertices[vertexIndex + 3] = 1.0f;
				this.vertices[vertexIndex + 4] = 1.0f;
				this.vertices[vertexIndex + 5] = 1.0f;
				this.vertices[vertexIndex + 6] = 1.0f;

				// Vec2 Texture Coordinate
				vertexIndex += 9;
			}

			int totalIndices = model.faces.Sum(face => (face.v.Length - 2) * 3);
			this.indices = new uint[totalIndices];
			int indicesIndex = 0;

			//	Triangle Fan
			//		Iterates through the faces of a model.
			//		For each face:
			//			iterates through each vertex (v) and vertex vertex texturecoord (vt) in the face
			//
			//		x and y from TextureCoordinate 
			//		
			//		

			for (int i = 0; i < model.faces.Length; i++)
			{
				Face face = model.faces[i];
				int numVertices = face.v.Length;

				for (int j = 0; j < numVertices; j++)
				{
					int vertexPos = face.v[j];
					int uvIndex = face.vt[j];

					//	mx + b
					//	Linear Equation used to map uv index offset in vertex buffer array
					int uI = (vertexPos * 9) + 7;
					int vI = uI + 1;

					// Add Texture Coordinate Vec2 to Vertex Buffer
					if (uvIndex >= 0 && uvIndex < model.texCoords.Length)
					{
						this.vertices[uI] = model.texCoords[uvIndex].uv.X;
						this.vertices[vI] = model.texCoords[uvIndex].uv.Y;
					}
				}

				for (int j = 1; j < numVertices - 1; j++)
				{
					this.indices[indicesIndex++] = (uint)face.v[0];
					this.indices[indicesIndex++] = (uint)face.v[j];
					this.indices[indicesIndex++] = (uint)face.v[j + 1];
				}
			}

			// Bind vertex data to the GPU
			Log.Default($"Binding vertex buffer (size: {vertices.Length}) to vertex buffer object");
			this.vbo = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbo);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, hint);

			Log.Default("Binding Vertex Array Object");
			this.vao = GL.GenVertexArray();
			GL.BindVertexArray(this.vao);
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbo);

			// Shader Data

			// Positional Data: Uniform Layout 1
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			// Vertex Colour: Uniform Layout 2
			GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 9 * sizeof(float), 3 * sizeof(float));
			GL.EnableVertexAttribArray(1);

			// TexCoord: Uniform Layout 3
			GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 9 * sizeof(float), 7 * sizeof(float));
			GL.EnableVertexAttribArray(2);

			// Index Buffer
			Log.Default($"Binding Index Buffer (size: {indices.Length}) to Element Buffer Object");
			this.ebo = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ebo);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

			GL.BindVertexArray(0);
			this.shaderProgram = shaderProgram;

			initialized = true;
		}


		public void Render()
		{
			Matrix4 view = Matrix4.CreateTranslation(Camera.main.Position.X, Camera.main.Position.Y, Camera.main.Position.Z);
			Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Camera.main.FOV), Viewport.Width / Viewport.Height, Camera.main.NearClip, Camera.main.FarClip);
			Matrix4 model = Matrix4.Identity * Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(Input.Mouse.Position.X * 0.1f)) * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(Input.Mouse.Position.Y * 0.1f)) * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(0));

			this.shaderProgram.SetUniform("model", model, true);
			this.shaderProgram.SetUniform("view", view, true);
			this.shaderProgram.SetUniform("projection", projection, true);
			this.shaderProgram.SetUniform("time", Time.time);

			GL.UseProgram(shaderProgram.ShaderProgramHandle);

			GL.BindVertexArray(this.vao);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ebo);

			GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

			ErrorCode error = GL.GetError();

			if (error != ErrorCode.NoError)
			{
				Log.Default($"OpenGL Error: {error}");
			}
		}

		~RenderObject() 
		{
			this.Dispose();
		}

		public void Dispose() 
		{
			if (this.disposed)
				return;

			if (initialized)
			{
				GL.DeleteBuffer(vbo);
				GL.DeleteBuffer(ebo);
				GL.DeleteVertexArray(vao);

				this.initialized = false;
			}

			this.disposed = true;
			GC.SuppressFinalize(this);
		}
	}
}
