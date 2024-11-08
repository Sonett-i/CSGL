using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;

namespace CSGL
{
	public class Cubemap : IDisposable
	{
		private float[] skyboxVertices = 
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

		private int vbo;
		private int vao;
		private int cubemapID;

		private ShaderProgram shader;
		private string[] facesCubeMap =
		{
			"Resources\\Textures\\Skybox\\right.jpg",
			"Resources\\Textures\\Skybox\\left.jpg",
			"Resources\\Textures\\Skybox\\top.jpg",
			"Resources\\Textures\\Skybox\\bottom.jpg",
			"Resources\\Textures\\Skybox\\front.jpg",
			"Resources\\Textures\\Skybox\\back.jpg"
		};

		public Cubemap()
		{
			this.shader = Resources.Shaders["skybox"].ShaderProgram;
			LoadCubeMap();
			Setup();
		}

		public void Draw(Matrix4 view, Matrix4 projection)
		{
			GL.DepthFunc(DepthFunction.Lequal);

			GL.UseProgram(shader.ShaderProgramHandle);

			int loc1 = GL.GetUniformLocation(shader.ShaderProgramHandle, "view");
			GL.UniformMatrix4(loc1, true, ref view);

			int loc2 = GL.GetUniformLocation(shader.ShaderProgramHandle, "projection");
			GL.UniformMatrix4(loc2, true, ref projection);

			GL.BindVertexArray(vao);
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.TextureCubeMap, cubemapID);

			GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
			var error = GL.GetError();

			GL.DepthFunc(DepthFunction.Less);
		}

		void Setup()
		{
			this.vbo = GL.GenBuffer();
			this.vao = GL.GenVertexArray();

			GL.BindVertexArray(vao);
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbo);

			GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * skyboxVertices.Length, skyboxVertices, BufferUsageHint.StaticDraw);
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

			
			GL.UseProgram(shader.ShaderProgramHandle);

			GL.Uniform1(GL.GetUniformLocation(shader.ShaderProgramHandle, "skybox"), 0);
		}

		private int LoadCubeMap()
		{
			cubemapID = GL.GenTexture();
			GL.BindTexture(TextureTarget.TextureCubeMap, cubemapID);

			StbImage.stbi_set_flip_vertically_on_load(0);
			for (int i = 0; i < facesCubeMap.Length; i++)
			{
				string currentFace = facesCubeMap[i];
				using (var stream = File.OpenRead(currentFace))
				{
					ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
					GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
				}
			}

			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

			StbImage.stbi_set_flip_vertically_on_load(1);
			return cubemapID;
		}

		~Cubemap() 
		{
			Dispose();
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			GL.DeleteBuffer(vbo);
			GL.DeleteVertexArray(vao);
		}
	}
}
