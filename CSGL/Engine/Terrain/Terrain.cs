using ContentPipeline;
using CSGL.Engine;
using Logging;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SharedLibrary;
using CSGL.Graphics;

namespace CSGL.Engine
{
	public class Terrain : Entity
	{
		float[,] heightmapData;
		public int Width { get; set; }
		public int Height { get; set; }

		public List<Matrix4> FoliagePosition = new List<Matrix4>();
		public int plantChance = 60;

		Vector3 offset = new Vector3(0, -600, 0);

		Instance foliage;

		public Terrain(string heightMap) : base("Terrain")
		{
			base.Lit = true;

			//this.AddComponent<Model>();

			Texture mapDiffuse = new Texture("mapdiffuse.png", TextureType.DIFFUSE, TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.UnsignedByte);

			TextureParameter[] difftexParams =
			{
				new TextureParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear),
				new TextureParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear),
				new TextureParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat),
				new TextureParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat),
				new TextureParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (int)TextureWrapMode.Repeat)
			};

			mapDiffuse.SetParameters(difftexParams);

			

			Texture mapSpecular = new Texture("mapspecular.png", TextureType.SPECULAR, TextureTarget.Texture2D, 1, PixelFormat.Rgba, PixelType.UnsignedByte);

			TextureParameter[] spectexParams =
{
				new TextureParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear),
				new TextureParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest),
				new TextureParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat),
				new TextureParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat),
				new TextureParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (int)TextureWrapMode.Repeat)
			};

			mapSpecular.SetParameters(spectexParams);

			this.AddTexture(mapDiffuse);
			this.AddTexture(mapSpecular);

			Shader defaultShader = ShaderManager.Shaders["default.shader"];
			this.heightmapData = GenerateHeightMap(Manifest.GetAsset<TextureAsset>(heightMap));

			Vertex[] vertices = GenerateVertices();
			uint[] indices = GenerateIndices();

			this.model.root = new MeshNode("terrain");
			this.model.root.Meshes.Add((new Mesh(vertices, indices, this.Textures, "Terrain")));
			this.model.root.Transform.position = offset;
			this.model.shader = defaultShader;

			ModelImporter importer = new ModelImporter(Manifest.GetAsset<ModelAsset>("Bush.fbx").FilePath);

			foliage = new Instance(importer.meshes[0].VAO, importer.meshes[0].VBO, importer.meshes[0].EBO, ShaderManager.Shaders["instance.shader"], FoliagePosition);
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

					Vector3 bitan = tangent;
					Vector2 uv = new Vector2((1.0f / Width) * x, (1.0f / Height) * z);

					Vertex vert = new Vertex(position, normal, tangent, bitan, uv);

					vertices.Add(vert);

					if (MathU.Random(0, 100) >= plantChance)
					{
						Transform transform = new Transform();
						transform.position = vert.position + offset;
						FoliagePosition.Add(transform.TRS());
					}
				}
			}

			Log.Info($"Terrain generated with {vertices.Count} vertices {FoliagePosition.Count} plants generated.");
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
			foliage.Draw();
			base.Render();
		}
	}
}

