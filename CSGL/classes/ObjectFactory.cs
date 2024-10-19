using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL
{
	public class ObjectFactory
	{
		public static RenderObject CreateTriangle(Vector3 origin, float width, ShaderProgram shaderProgram)
		{
			float halfWidth = width / 2f;

			float[] vertices = new float[]
			{
				origin.X, origin.Y + halfWidth, origin.Z,               0.0f, 0.0f, 1.0f, 1.0f, // v0
				origin.X + halfWidth, origin.Y - halfWidth, origin.Z,   0.0f, 1.0f, 0.0f, 1.0f, // v1
				origin.X - halfWidth, origin.Y - halfWidth, origin.Z,	1.0f, 0.0f, 0.0f, 1.0f, // v2
			};

			uint[] indices = new uint[]
			{
				0, 1, 2
			};

			return new RenderObject(vertices, indices, shaderProgram, OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
		}

		public static RenderObject CreateQuad(Vector3 origin, float width, float height, ShaderProgram shaderProgram)
		{
			float halfWidth = width / 2;
			float halfHeight = height / 2;

			float[] vertices = new float[]
			{
				origin.X - halfHeight, origin.Y + halfHeight, origin.Z,  0.0f, 0.0f, 1.0f, 1.0f, // v0
				origin.X + halfHeight, origin.Y + halfHeight, origin.Z,  0.0f, 0.0f, 1.0f, 1.0f, // v1
				origin.X + halfWidth, origin.Y - halfHeight, origin.Z,   0.0f, 1.0f, 0.0f, 1.0f, // v2
				origin.X - halfWidth, origin.Y - halfHeight, origin.Z,   1.0f, 0.0f, 0.0f, 1.0f, // v3
			};

			uint[] indices = new uint[]
			{
				0, 1, 2,
				0, 2, 3
			};

			return new RenderObject(vertices, indices, shaderProgram, OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
		}

		public static RenderObject CreateCube(Vector3 origin, float size, ShaderProgram shaderProgram, Color4 colour)
		{
			float halfSize = size / 2;

			float[] vertices = new float[]
			{
				// Front face
				origin.X - halfSize, origin.Y + halfSize, origin.Z + halfSize,  1.0f, 0.0f, 0.0f, 1.0f, // v0
				origin.X + halfSize, origin.Y + halfSize, origin.Z + halfSize,  0.0f, 1.0f, 0.0f, 1.0f, // v1
				origin.X + halfSize, origin.Y - halfSize, origin.Z + halfSize,  0.0f, 0.0f, 1.0f, 1.0f, // v2
				origin.X - halfSize, origin.Y - halfSize, origin.Z + halfSize,  1.0f, 1.0f, 0.0f, 1.0f, // v3

				// Back face
				origin.X - halfSize, origin.Y + halfSize, origin.Z - halfSize,  1.0f, 1.0f, 0.0f, 1.0f, // v4
				origin.X + halfSize, origin.Y + halfSize, origin.Z - halfSize,  0.0f, 0.0f, 1.0f, 1.0f, // v5
				origin.X + halfSize, origin.Y - halfSize, origin.Z - halfSize,  0.0f, 1.0f, 0.0f, 1.0f, // v6
				origin.X - halfSize, origin.Y - halfSize, origin.Z - halfSize,  1.0f, 0.0f, 0.0f, 1.0f, // v7
			};

			uint[] indices = new uint[]
			{
				// Front face
				0, 1, 2, 0, 2, 3,
				// Back face
				4, 5, 6, 4, 6, 7,
				// Left face
				0, 4, 7, 0, 7, 3,
				// Right face
				1, 5, 6, 1, 6, 2,
				// Top face
				0, 1, 5, 0, 5, 4,
				// Bottom face
				3, 2, 6, 3, 6, 7
			};

			return new RenderObject(vertices, indices, shaderProgram, OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
		}
	}
}
