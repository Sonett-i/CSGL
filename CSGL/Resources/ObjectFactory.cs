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
				origin.X - halfWidth, origin.Y - halfWidth, origin.Z,	1.0f, 0.0f, 0.0f, 1.0f,
				origin.X + halfWidth, origin.Y - halfWidth, origin.Z,   0.0f, 1.0f, 0.0f, 1.0f,
				origin.X, origin.Y + halfWidth, origin.Z,				0.0f, 0.0f, 1.0f, 1.0f,
			};

			uint[] indices = new uint[]
			{
				0, 1, 2
			};

			return new RenderObject(vertices, indices, shaderProgram, OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
		}

	}
}
