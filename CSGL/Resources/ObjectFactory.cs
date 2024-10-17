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
		public static Vertex[] CreateSolidCube(float side, Color4 colour)
		{
			side = side / 2f;

			Vertex[] vertices =
			{
				new Vertex(new Vector4(-side, -side, -side, 1.0f),   colour), // v0
				new Vertex(new Vector4(-side, -side, side, 1.0f),    colour), // v1
				new Vertex(new Vector4(-side, side, -side, 1.0f),    colour), // v2
				new Vertex(new Vector4(-side, side, -side, 1.0f),    colour), // v3
				new Vertex(new Vector4(-side, -side, side, 1.0f),    colour), // v4
				new Vertex(new Vector4(-side, side, side, 1.0f),     colour), // v5

				new Vertex(new Vector4(side, -side, -side, 1.0f),    colour), // v6
				new Vertex(new Vector4(side, side, -side, 1.0f),     colour), // v7
				new Vertex(new Vector4(side, -side, side, 1.0f),     colour), // v8
				new Vertex(new Vector4(side, -side, side, 1.0f),     colour), // v9
				new Vertex(new Vector4(side, side, -side, 1.0f),     colour), // v10
				new Vertex(new Vector4(side, side, side, 1.0f),      colour), // v11

				new Vertex(new Vector4(-side, -side, -side, 1.0f),   colour), // v12
				new Vertex(new Vector4(side, -side, -side, 1.0f),    colour), // v13
				new Vertex(new Vector4(-side, -side, side, 1.0f),    colour), // v14
				new Vertex(new Vector4(-side, -side, side, 1.0f),    colour), // v15
				new Vertex(new Vector4(side, -side, -side, 1.0f),    colour), // v16
				new Vertex(new Vector4(side, -side, side, 1.0f),     colour), // v17

				new Vertex(new Vector4(-side, side, -side, 1.0f),    colour),
				new Vertex(new Vector4(-side, side, side, 1.0f),     colour),
				new Vertex(new Vector4(side, side, -side, 1.0f),     colour),
				new Vertex(new Vector4(side, side, -side, 1.0f),     colour),
				new Vertex(new Vector4(-side, side, side, 1.0f),     colour),
				new Vertex(new Vector4(side, side, side, 1.0f),      colour),

				new Vertex(new Vector4(-side, -side, -side, 1.0f),   colour),
				new Vertex(new Vector4(-side, side, -side, 1.0f),    colour),
				new Vertex(new Vector4(side, -side, -side, 1.0f),    colour),
				new Vertex(new Vector4(side, -side, -side, 1.0f),    colour),
				new Vertex(new Vector4(-side, side, -side, 1.0f),    colour),
				new Vertex(new Vector4(side, side, -side, 1.0f),     colour),

				new Vertex(new Vector4(-side, -side, side, 1.0f),    colour),
				new Vertex(new Vector4(side, -side, side, 1.0f),     colour),
				new Vertex(new Vector4(-side, side, side, 1.0f),     colour),
				new Vertex(new Vector4(-side, side, side, 1.0f),     colour),
				new Vertex(new Vector4(side, -side, side, 1.0f),     colour),
				new Vertex(new Vector4(side, side, side, 1.0f),      colour),
			};

			return vertices;
		}

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

			/*
			float[] vertices = new float[]
			{
				// Front face
				origin.X - halfSize, origin.Y + halfSize, origin.Z + halfSize,  colour.R, colour.G, colour.B, colour.A, // v0
				origin.X + halfSize, origin.Y + halfSize, origin.Z + halfSize,  colour.R, colour.G, colour.B, colour.A, // v1
				origin.X + halfSize, origin.Y - halfSize, origin.Z + halfSize,  colour.R, colour.G, colour.B, colour.A, // v2
				origin.X - halfSize, origin.Y - halfSize, origin.Z + halfSize,  colour.R, colour.G, colour.B, colour.A, // v3

				// Back face
				origin.X - halfSize, origin.Y + halfSize, origin.Z - halfSize,  colour.R, colour.G, colour.B, colour.A, // v4
				origin.X + halfSize, origin.Y + halfSize, origin.Z - halfSize,  colour.R, colour.G, colour.B, colour.A, // v5
				origin.X + halfSize, origin.Y - halfSize, origin.Z - halfSize,  colour.R, colour.G, colour.B, colour.A, // v6
				origin.X - halfSize, origin.Y - halfSize, origin.Z - halfSize,  colour.R, colour.G, colour.B, colour.A, // v7
			};
			*/

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

		public static RenderObject CreateTorus(Vector3 origin, float majorRadius, float minorRadius, int majorSegments, int minorSegments, ShaderProgram shaderProgram)
		{
			List<float> vertices = new List<float>();
			List<uint> indices = new List<uint>();

			for (int i = 0; i <= majorSegments; i++)
			{
				float theta = i * MathF.PI * 2 / majorSegments;
				float cosTheta = MathF.Cos(theta);
				float sinTheta = MathF.Sin(theta);

				for (int j = 0; j <= minorSegments; j++)
				{
					float phi = j * MathF.PI * 2 / minorSegments;
					float cosPhi = MathF.Cos(phi);
					float sinPhi = MathF.Sin(phi);

					// Calculate the position of the vertex
					float x = (majorRadius + minorRadius * cosPhi) * cosTheta;
					float y = (majorRadius + minorRadius * cosPhi) * sinTheta;
					float z = minorRadius * sinPhi;

					// Calculate normal direction
					float nx = cosPhi * cosTheta;
					float ny = cosPhi * sinTheta;
					float nz = sinPhi;

					// Add vertex position and normal to the list
					vertices.Add(origin.X + x);
					vertices.Add(origin.Y + y);
					vertices.Add(origin.Z + z);
					vertices.Add(nx);
					vertices.Add(ny);
					vertices.Add(nz);
					vertices.Add(1.0f); // Placeholder for w (e.g., can be used for other attributes like color)

					// Generate indices for triangle strip
					int nextI = (i + 1) % (majorSegments + 1);
					int nextJ = (j + 1) % (minorSegments + 1);

					indices.Add((uint)(i * (minorSegments + 1) + j));
					indices.Add((uint)(nextI * (minorSegments + 1) + j));
					indices.Add((uint)(i * (minorSegments + 1) + nextJ));

					indices.Add((uint)(i * (minorSegments + 1) + nextJ));
					indices.Add((uint)(nextI * (minorSegments + 1) + j));
					indices.Add((uint)(nextI * (minorSegments + 1) + nextJ));
				}
			}

			return new RenderObject(vertices.ToArray(), indices.ToArray(), shaderProgram, OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
		}


	}
}
