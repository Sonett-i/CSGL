using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using StbImageSharp;
using CSGL.Graphics;
using ContentPipeline;
using Logging;

namespace CSGL.Engine
{
	public class Cubemap : IDisposable
	{
		private readonly float[] skyboxVertices =
		{
			// positions          
			-1.0f,  1.0f, -1.0f,
			-1.0f, -1.0f, -1.0f,
			 1.0f, -1.0f, -1.0f,
			 1.0f, -1.0f, -1.0f,
			 1.0f,  1.0f, -1.0f,
			-1.0f,  1.0f, -1.0f,

			-1.0f, -1.0f,  1.0f,
			-1.0f, -1.0f, -1.0f,
			-1.0f,  1.0f, -1.0f,
			-1.0f,  1.0f, -1.0f,
			-1.0f,  1.0f,  1.0f,
			-1.0f, -1.0f,  1.0f,

			 1.0f, -1.0f, -1.0f,
			 1.0f, -1.0f,  1.0f,
			 1.0f,  1.0f,  1.0f,
			 1.0f,  1.0f,  1.0f,
			 1.0f,  1.0f, -1.0f,
			 1.0f, -1.0f, -1.0f,

			-1.0f, -1.0f,  1.0f,
			-1.0f,  1.0f,  1.0f,
			 1.0f,  1.0f,  1.0f,
			 1.0f,  1.0f,  1.0f,
			 1.0f, -1.0f,  1.0f,
			-1.0f, -1.0f,  1.0f,

			-1.0f,  1.0f, -1.0f,
			 1.0f,  1.0f, -1.0f,
			 1.0f,  1.0f,  1.0f,
			 1.0f,  1.0f,  1.0f,
			-1.0f,  1.0f,  1.0f,
			-1.0f,  1.0f, -1.0f,

			-1.0f, -1.0f, -1.0f,
			-1.0f, -1.0f,  1.0f,
			 1.0f, -1.0f, -1.0f,
			 1.0f, -1.0f, -1.0f,
			-1.0f, -1.0f,  1.0f,
			 1.0f, -1.0f,  1.0f
		};

		int VAO;
		int VBO;
		int ID;

		Shader shader = null!;

		public Cubemap(params string[] textures)
		{
			this.ID = LoadCubeMap(textures);
			Setup();
		}

		int LoadCubeMap(string[] textures)
		{
			this.ID = GL.GenTexture();

			GL.BindTexture(TextureTarget.TextureCubeMap, ID);

			StbImage.stbi_set_flip_vertically_on_load(0);

			for (int i = 0; i < 6; i++)
			{
				TextureAsset tex = Manifest.GetAsset<TextureAsset>(textures[i]);

				using (Stream stream = File.OpenRead(tex.FilePath))
				{
					ImageResult result = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
					GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, result.Width, result.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, result.Data);

					ErrorCode error = GL.GetError();
					if (error != ErrorCode.NoError)
					{
						Log.Error($"Error with face {i}: {tex}: {error} ");
					}
				}
			}

			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

			return this.ID;
		}

		void Setup()
		{
			VBO = GL.GenBuffer();
			VAO = GL.GenVertexArray();

			GL.BindVertexArray(VAO);
			GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

			GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * skyboxVertices.Length, skyboxVertices, BufferUsageHint.StaticDraw);
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

			this.shader = ShaderManager.Shaders["skybox.shader"];

			this.shader.SetUniform("skybox", 0);
		}

		// CODE

		public void Draw()
		{
			GL.Disable(EnableCap.CullFace);
			

			shader.Activate();

			GL.DepthFunc(DepthFunction.Lequal);


			shader.SetUniform("view", Camera.main.SkyMatrix());
			shader.SetUniform("projection", Camera.main.ProjectionMatrix);

			GL.BindVertexArray(VAO);

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.TextureCubeMap, ID);

			GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

			ErrorCode error = GL.GetError();

			if (error != ErrorCode.NoError)
				Log.GL($"Error: {error}");

			GL.DepthFunc(DepthFunction.Less);
			GL.Enable(EnableCap.CullFace);
		}
		
		// Cleanup
		~Cubemap()
		{
			Dispose();
		}

		public void Dispose()
		{
			Log.GL($"Disposing {this}");
			GL.DeleteTexture(this.ID);
			GL.DeleteBuffer(VAO);
			GL.DeleteBuffer(VBO);
			GC.SuppressFinalize(this);
		}

	}
}
