﻿using System;
using System.Numerics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CSGL
{
	//	Vertex Buffer Array Format
	//	name	| index	|  data	| size
	//	pos		| 0		|  xyz	| 3
	//	colour	| 3		|  rgba	| 4
	//	normals | 7		|  xyz	| 3
	//	uv		| 10	|  uv	| 2

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

		public RenderObject(Model model, ShaderProgram shaderProgram, BufferUsageHint hint = BufferUsageHint.StaticDraw)
		{
			this.hint = hint;
			this.name = model.name;

			// Vertices array stores position, color, and texture coordinates
			this.vertices = new float[model.vertices.Length * 12];
			int vertexIndex = 0;

			//	Fills Vertex Data

			for (int i = 0; i < model.vertices.Length; i++)
			{
				// Vec3 Vertex Position
				this.vertices[vertexIndex] = model.vertices[i].x;       //0
				this.vertices[vertexIndex + 1] = model.vertices[i].y;   //1
				this.vertices[vertexIndex + 2] = model.vertices[i].z;   //2

				// Vec4 Colour (white by default)
				this.vertices[vertexIndex + 3] = 1.0f;                  //3
				this.vertices[vertexIndex + 4] = 1.0f;                  //4
				this.vertices[vertexIndex + 5] = 1.0f;                  //5
				this.vertices[vertexIndex + 6] = 1.0f;                  //6

				// Vec2 Texture Coordinate
				vertexIndex += 12;
			}

			int totalIndices = model.faces.Sum(face => (face.v.Length - 2) * 3);
			this.indices = new uint[totalIndices];
			int indicesIndex = 0;

			for (int i = 0; i < model.faces.Length; i++)
			{
				Face face = model.faces[i];
				int numVertices = face.v.Length;

				for (int j = 0; j < numVertices; j++)
				{
					int vertexPos = face.v[j];
					int uvIndex = face.vt[j];
					int normalIndex = face.vn[j];

					// Vec3 Vertex Normals
					int vnI = (vertexPos * 12) + 7;
					this.vertices[vnI] = model.normals[normalIndex].normal.X;
					this.vertices[vnI + 1] = model.normals[normalIndex].normal.Y;
					this.vertices[vnI + 2] = model.normals[normalIndex].normal.Z;

					//	mx + b
					//	Linear Equation used to map uv index offset in vertex buffer array
					int uvI = (vertexPos * 12) + 10; // TexCoord offset


					// Add Texture Coordinate Vec2 to Vertex Buffer
					if (uvIndex >= 0 && uvIndex < model.texCoords.Length)
					{
						this.vertices[uvI] = model.texCoords[uvIndex].uv.X;	//10
						this.vertices[uvI + 1] = model.texCoords[uvIndex].uv.Y;	//11
					}
				}

				// Create Triangles by connecting the current face's root vertex to 2 others on the current face
				//	Skips root vertex
				for (int j = 1; j < numVertices - 1; j++)
				{
					this.indices[indicesIndex++] = (uint)face.v[0]; // root vertex in current face
					this.indices[indicesIndex++] = (uint)face.v[j]; // vertex at current index
					this.indices[indicesIndex++] = (uint)face.v[j + 1]; // neighboring vertex 
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

			// Positional Data (vec3): Uniform Layout 0
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			// Vertex Colour (vec4): Uniform Layout 1
			GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 12 * sizeof(float), 3 * sizeof(float));
			GL.EnableVertexAttribArray(1);

			// Normal Data (vec3): Uniform Layout 2
			GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 12 * sizeof(float), 7 * sizeof(float));
			GL.EnableVertexAttribArray(2);

			// TexCoord (vec2): Uniform Layout 3
			GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, 12 * sizeof(float), 10 * sizeof(float));
			GL.EnableVertexAttribArray(3);

			// Index Buffer
			Log.Default($"Binding Index Buffer (size: {indices.Length}) to Element Buffer Object");
			this.ebo = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ebo);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, hint);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

			GL.BindVertexArray(0);
			this.shaderProgram = shaderProgram;

			initialized = true;
		}

		public void Render()
		{
			Matrix4 view = Matrix4.CreateTranslation(Camera.main.Position.X, Camera.main.Position.Y, Camera.main.Position.Z);
			Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Camera.main.FOV), Viewport.Width / Viewport.Height, Camera.main.NearClip, Camera.main.FarClip);
			Matrix4 model = Matrix4.Identity * Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians((-Input.Mouse.Position.Y + 100) * 0.1f)) * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians((Input.Mouse.Position.X + 100) * 0.1f)) * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(0));

			this.shaderProgram.SetUniform("model", model, true);
			this.shaderProgram.SetUniform("view", view, true);
			this.shaderProgram.SetUniform("projection", projection, true);
			this.shaderProgram.SetUniform("time", Time.time);

			GL.UseProgram(shaderProgram.ShaderProgramHandle);

			GL.BindVertexArray(this.vao);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ebo);

			GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

			// If there is an issue with rendering, output to console
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
