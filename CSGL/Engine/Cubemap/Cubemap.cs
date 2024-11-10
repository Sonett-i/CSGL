using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;

namespace CSGL
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

		// OpenGL handles
		private int vbo;
		private int vao;
		private int cubemapID;

		private Shader shader;

		// Texture paths for the cubemap
		private readonly string[] facesCubeMap =
		{
			"right.jpg", // +X
			"left.jpg", // -X
			"top.jpg", // +Y
			"bottom.jpg", // -Y
			"front.jpg", // +Z
			"back.jpg"  // -Z
		};

		Texture2D[] Cfaces;

		public Cubemap()
		{
			Cfaces = new Texture2D[6];

			for (int i = 0; i < 6; i++)
			{
				if (i < facesCubeMap.Length)
				{
					Cfaces[i] = Resources.Textures[facesCubeMap[i]];
				}
				else
				{
					Cfaces[i] = Resources.Textures[facesCubeMap[i % facesCubeMap.Length]];
				}
			}

			this.shader = Resources.Shaders["skybox"];
			LoadCubeMap();
			Setup();
		}

		public Cubemap(string[] faces)
		{
			facesCubeMap = new string[6];

			Cfaces = new Texture2D[6];

			for (int i = 0; i < 6; i++)
			{
				if (i < faces.Length)
				{
					Cfaces[i] = Resources.Textures[faces[i]];
				}
				else
				{
					Cfaces[i] = Resources.Textures[faces[i % faces.Length]];
				}
			}

			this.shader = Resources.Shaders["skybox"];
			LoadCubeMap();
			Setup();
		}

		// Initialize vertex array and buffer elements 
		void Setup()
		{
			this.vbo = GL.GenBuffer();
			this.vao = GL.GenVertexArray();

			GL.BindVertexArray(vao);
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbo);

			GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * skyboxVertices.Length, skyboxVertices, BufferUsageHint.StaticDraw);
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

			GL.UseProgram(shader.ShaderProgram.ShaderProgramHandle);

			GL.Uniform1(shader.Uniforms["skybox"].Location, 0);
		}

		// Loads input cubemap textures into cubemap object
		private int LoadCubeMap()
		{
			cubemapID = GL.GenTexture();
			GL.BindTexture(TextureTarget.TextureCubeMap, cubemapID);

			for (int i = 0; i < Cfaces.Length; i++)
			{
				GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, Cfaces[i].Width, Cfaces[i].Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, Cfaces[i].data);

				ErrorCode error = GL.GetError();
				if (error != ErrorCode.NoError)
				{
					Log.Error($"Error with face {i}: {Cfaces}: {error} ");
				}
			}
			SetTextureParameters();

			return cubemapID;
		}
		
		// Set Texture Parameters for filtering and wrapping
		void SetTextureParameters()
		{
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
		}

		// Draw cubemap using camera view and projection matrices
		public void Draw()
		{
			GL.DepthFunc(DepthFunction.Lequal); // set depth test to less than equal so the skybox doesn't render in front of anything

			GL.UseProgram(shader.ShaderProgram.ShaderProgramHandle);

			// Set shader uniforms
			GL.UniformMatrix4(shader.Uniforms["view"].Location, true, ref Camera.main.m_View);
			GL.UniformMatrix4(shader.Uniforms["projection"].Location, true, ref Camera.main.m_Projection);

			// Bind vertex array
			GL.BindVertexArray(vao);
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.TextureCubeMap, cubemapID);

			GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

			ErrorCode error = GL.GetError();
			if (error != ErrorCode.NoError)
			{
				Log.Error($"Error Drawing Cubemap: {error}");
			}

			GL.DepthFunc(DepthFunction.Less);
		}

		// Cleanup
		~Cubemap() 
		{
			Dispose();
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			GL.DeleteBuffer(vbo);
			GL.DeleteVertexArray(vao);
			GL.DeleteTexture(cubemapID);
		}
	}
}

// https://www.khronos.org/opengl/wiki/texture
// https://www.khronos.org/opengl/wiki/Cubemap_Texture