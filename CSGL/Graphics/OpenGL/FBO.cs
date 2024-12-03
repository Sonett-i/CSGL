using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Logging;

namespace CSGL.Graphics
{
	// FBO = Frame Buffer Object
	public class FBO : IDisposable
	{

		float[] vertices =
		{
			1.0f, -1.0f,	1.0f, 0.0f,
			-1.0f, -1.0f,	0.0f, 0.0f,
			-1.0f, 1.0f,	0.0f, 1.0f,

			1.0f, 1.0f,		1.0f, 1.0f,
			1.0f, -1.0f,	1.0f, 0.0f,
			-1.0f, 1.0f,	0.0f, 1.0f
		};

		public int ID;
		int framebufferTexture;
		bool initialized = false;

		int RBO;

		public FBO()
		{
			ID = GL.GenFramebuffer();
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, this.ID);

			framebufferTexture = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, framebufferTexture);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, WindowConfig.Width, WindowConfig.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, 0);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, framebufferTexture, 0);

			RBO = GL.GenRenderbuffer();
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RBO);
			GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, framebufferTexture, 0);
			GL.DrawBuffer(DrawBufferMode.None);
			GL.ReadBuffer(ReadBufferMode.None);

			FramebufferErrorCode fboStatus = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);

			if (fboStatus != FramebufferErrorCode.FramebufferComplete)
			{
				Log.GL("Error: Framebuffer not complete: " + fboStatus.ToString());
			}

			initialized = true;

			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

		}

		public void Bind()
		{
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, this.ID);
		}

		public void Unbind()
		{
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
		}

		public void Dispose()
		{
			if (initialized)
			{
				GL.DeleteFramebuffer(ID);
				GL.DeleteTexture(framebufferTexture);
				GL.DeleteRenderbuffer(RBO);
			}
				

			GC.SuppressFinalize(this);
		}

	}
}

// https://www.youtube.com/watch?v=Ut6poChkSjA
