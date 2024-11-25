using System;
using ContentPipeline;
using ContentPipeline.Extensions;
using OpenTK.Graphics.OpenGL;
using Logging;
using CSGL.Engine;

namespace CSGL
{
	public static class EngineConfig
	{
		public static string Version = "0.0.0";
		public static string Name = "CSGL";
	}

	public static class WindowConfig
	{
		public static int Width = 1024;
		public static int Height = 768;
		public static bool Vsync = false;
		public static float TargetFrameRate = 60.0f;

		public static float AspectRatio
		{
			get
			{
				return Width / Height;
			}
		}
	}

	public static class Config
	{
		public static void Set(INI config)
		{
			try
			{
				// Set Engine Config
				CSGL.EngineConfig.Name = config.Contents["Engine"]["Name"];
				CSGL.EngineConfig.Version = config.Contents["Engine"]["Version"];

				// Set Window Config
				CSGL.WindowConfig.Width = int.Parse(config.Contents["Window"]["Width"]);
				CSGL.WindowConfig.Height = int.Parse(config.Contents["Window"]["Height"]);
				CSGL.WindowConfig.Vsync = bool.Parse(config.Contents["Window"]["Vsync"]);
				CSGL.WindowConfig.TargetFrameRate = float.Parse(config.Contents["Window"]["TargetFrameRate"]);
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message);
			}
		}

		public static void SetOpenGLConfig()
		{
			TextureManager.MaxTextures = GL.GetInteger(GetPName.MaxTextureImageUnits);
			TextureManager.MaxTextureSize = GL.GetInteger(GetPName.MaxTextureSize);
		}
	}
}
