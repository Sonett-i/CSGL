using ContentPipeline;
using ContentPipeline.Components;
using CSGL.Engine;
using Logging;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SharedLibrary;

namespace CSGL.Engine
{
	public class Terrain : Entity
	{
		float[,] heightmapData;
		public int Width { get; set; }
		public int Height { get; set; }

		public Terrain(string heightMap) : base("Terrain")
		{
			base.Lit = true;
			Texture mapDiffuse = new Texture("mapdiffuse.png", TextureType.DIFFUSE, TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.UnsignedByte);

			TextureParameter[] texParams =
			{
				new TextureParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear),
				new TextureParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear),
				new TextureParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat),
				new TextureParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat),
				new TextureParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (int)TextureWrapMode.Repeat)
			};

			mapDiffuse.SetParameters(texParams);

			Shader defaultShader = ShaderManager.Shaders["default.shader"];
			this.AddTexture(mapDiffuse);
			this.AddTexture(new Texture("mapspecular.png", TextureType.SPECULAR, TextureTarget.Texture2D, 1, PixelFormat.Red, PixelType.UnsignedByte));
			this.heightmapData = GenerateHeightMap(Manifest.GetAsset<TextureAsset>(heightMap));

			Vertex[] vertices = GenerateVertices();
			uint[] indices = GenerateIndices();

			Mesh mesh = new Mesh(vertices, indices, this.Textures, defaultShader);

			this.model.AddMesh(mesh);

			this.transform.position = new Vector3(0, -600, 0);
		}

		uint[] GenerateIndices()
		{
			List<uint> indices = new List<uint>();

			for (uint i = 0; i < (Width - 1); i++)      // for each row a.k.a. each strip
			{
				for (uint j = 0; j < (Height - 1); j++)      // for each column
				{
					indices.Add((uint)(j * Width) + i);
					indices.Add((uint)(j * Width) + i + 1);
					indices.Add((uint)((j + 1) * Width) + i + 1);

					indices.Add((uint)((j + 1) * Width) + i + 1);
					indices.Add((uint)((j + 1) * Width) + i);
					indices.Add((uint)(j * Width) + i);
				}
			}

			return indices.ToArray();
		}

		Vertex[] GenerateVertices()
		{
			Log.Info("Generating terrain");

			List<Vertex> vertices = new List<Vertex>();

			for (uint x = 0; x < Width; x++)
			{
				for (uint z = 0; z < Height; z++)
				{
					float h = heightmapData[x, z];

					// Smooth heightmap value with neighbours (expensive)
					int smooth = 4;
					if (x > smooth && x < Width - 1 - smooth && z > smooth && z < Height - 1 - smooth)
					{
						float sum = 0;
						float e = 0;
						for (int i = -smooth; i < smooth; i++)
						{
							for (int j = -smooth; j < smooth; j++)
							{
								sum += heightmapData[x + i, z + j];
								e++;
							}
						}
						h = sum / e;
					}

					float hLeft = (x > 0) ? heightmapData[x - 1, z] : h;
					float hRight = (x > Width - 1) ? heightmapData[x + 1, z] : h;
					float hDown = (z > 0) ? heightmapData[x, z - 1] : h;
					float hUp = (z > Height - 1) ? heightmapData[x, z + 1] : h;

					Vector3 tangentX = new Vector3(2.0f, hRight - hLeft, 0.0f);
					Vector3 tangentZ = new Vector3(0.0f, hUp - hDown, 2.0f);

					// position coordinates
					float TILE = 1.0f;
					float xpos = (-Width / 2.0f) + x;
					float zpos = (-Height / 2.0f) + z;

					Vector3 position = new Vector3(xpos * TILE, h, zpos * TILE);
					Vector3 normal = Vector3.Cross(tangentX, tangentZ).Normalized();
					Vector3 tangent = new Vector3(tangentX.X, tangentX.Y, tangentX.Z);

					Vector2 uv = new Vector2((1.0f / Width) * x, (1.0f / Height) * z);

					vertices.Add(new Vertex(position, normal, tangent, uv));
				}
			}

			Log.Info($"Terrain generated with {vertices.Count} vertices");
			return vertices.ToArray();
		}

		float[,] GenerateHeightMap(TextureAsset texAsset)
		{
			this.Width = texAsset.Width;
			this.Height = texAsset.Height;

			byte[] imageData = Manifest.GetAsset<TextureAsset>("heightmap.png").Load(4);

			float[,] heightmapData = new float[this.Width, this.Height];

			for (uint x = 0; x < this.Width; x++)
			{
				for (uint z = 0; z < this.Height; z++)
				{
					long index = (x + Width * z) * 4;
					byte texel = (imageData[(x + Width * z) * 4]);
					float r = imageData[(x + Width * z) * 4] + 0;
					float g = imageData[(x + Width * z) * 4] + 1;
					float b = imageData[(x + Width * z) * 4] + 2;
					float h = ((r + g + b) / 3) * 4f;
					heightmapData[x, z] = h;
				}
			}

			return heightmapData;
		}

		public override void Render()
		{
			base.Render();
		}
	}
}

