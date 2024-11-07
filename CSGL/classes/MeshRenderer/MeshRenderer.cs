﻿using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL
{
	public class MeshRenderer
	{
		public Material material;
		public MeshFilter MeshFilter;
		public BufferUsageHint BufferUsageHint;

		private VertexBuffer[] Buffers;

		public Matrix4 model_Matrix;

		public MeshRenderer(MeshFilter MeshFilter, Material material, BufferUsageHint hint = BufferUsageHint.StaticDraw) 
		{
			this.MeshFilter = MeshFilter;
			this.material = material;
			this.BufferUsageHint = hint;

			this.Buffers = new VertexBuffer[MeshFilter.Meshes.Length];

			for (int i = 0; i < MeshFilter.Meshes.Length; i++)
			{
				Buffers[i] = new VertexBuffer(MeshFilter.Meshes[i]);
				Log.Default($"OpenGL: Bound model: {MeshFilter.Name} mesh: {MeshFilter.Meshes[i].Name} (Vertices: {MeshFilter.Meshes[i].VertexCount}, Faces: {MeshFilter.Meshes[i].Faces}) Size: {CSGLU.KiB(Buffers[i].Size)} KiB");
			}
		}

		public void Render()
		{
			this.material.MVP(model_Matrix, Camera.main.m_View, Camera.main.m_Projection);
			this.material.Render();

			GL.UseProgram(this.material.Shader.ShaderProgramHandle);
			for (int i = 0; i < this.Buffers.Length; i++)
			{
				Buffers[i].Render();
			}
		}

		public void Dispose()
		{
			foreach (VertexBuffer buffer in Buffers)
			{
				buffer.Dispose();
			}
		}

		~MeshRenderer()
		{
			this.Dispose();
		}
	}

	public class VertexBuffer : IDisposable
	{
		private bool initialized = false;
		private bool disposed = false;

		public readonly float[] buffer;
		public readonly uint[] indices;

		private int vbo;
		private int vao;
		private int ebo;

		private BufferUsageHint BufferUsageHint;

		public int Size
		{
			get
			{
				return buffer.Length * sizeof(float) + indices.Length * sizeof(int);
			}
		}

		public VertexBuffer(Mesh mesh, BufferUsageHint hint = BufferUsageHint.StaticDraw)
		{
			this.buffer = new float[mesh.Vertices.Length * 12];
			this.indices = new uint[mesh.Indices.Length];
			this.BufferUsageHint = hint;

			int vIndex = 0;

			for (int i = 0; i < mesh.Vertices.Length; i++)
			{
				vIndex = i * 12;

				Vertex v = mesh.Vertices[i];

				// Vertex Position
				buffer[vIndex] = v.Position.X;
				buffer[vIndex + 1] = v.Position.Y;
				buffer[vIndex + 2] = v.Position.Z;

				// Vertex Colour
				buffer[vIndex + 3] = 1.0f;
				buffer[vIndex + 4] = 1.0f;
				buffer[vIndex + 5] = 1.0f;
				buffer[vIndex + 6] = 1.0f;

				// Vertex Normals
				buffer[vIndex + 7] = v.Normal.X;
				buffer[vIndex + 8] = v.Normal.Y;
				buffer[vIndex + 9] = v.Normal.Z;

				// Texture coords
				buffer[vIndex + 10] = v.UV.X;
				buffer[vIndex + 11] = v.UV.Y;
			}

			this.indices = mesh.Indices;

			// OpenGL
			this.vbo = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbo);
			GL.BufferData(BufferTarget.ArrayBuffer, this.buffer.Length * sizeof(float), buffer, BufferUsageHint);

			this.vao = GL.GenVertexArray();
			GL.BindVertexArray(this.vao);
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbo); // might be duplicate

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

			this.ebo = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ebo);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

			GL.BindVertexArray(0);

			initialized = true;
		}

		public void Render()
		{
			GL.BindVertexArray(this.vao);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ebo);
			GL.DrawElements(BeginMode.Triangles, this.indices.Length, DrawElementsType.UnsignedInt, 0);

			ErrorCode error = GL.GetError();

			if (error != ErrorCode.NoError)
			{
				Log.Error($"OpenGL Error: {error}");
			}
		}

		public void Dispose()
		{
			if (disposed)
				return;

			if (!initialized)
				return;

			try
			{
				if (this.vbo != 0)
					GL.DeleteBuffer(vbo);

				if (this.vao != 0)
					GL.DeleteVertexArray(vao);

				if (this.ebo != 0)
					GL.DeleteBuffer(ebo);

			}
			catch (AccessViolationException ex)
			{
				Log.Error(ex.Message);
			}

			disposed = true;
			GC.SuppressFinalize(this);
		}

		~VertexBuffer()
		{
			this.Dispose();
		}
	}
}