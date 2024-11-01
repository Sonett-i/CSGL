using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL
{
	public static class EditorConfig
	{
		//	Config
		public static string ConfigDirectory = Environment.CurrentDirectory + "\\Config\\";

		//	Resources
		public static string ShaderDirectory = Environment.CurrentDirectory + "\\Resources\\Shaders\\";
		public static string ModelDirectory = Environment.CurrentDirectory + "\\Resources\\Models\\";
		public static string TextureDirectory = Environment.CurrentDirectory + "\\Resources\\Textures\\";
		public static string MaterialDirectory = Environment.CurrentDirectory + "\\Resources\\Materials\\";

		//	GameObjects

		//	Scenes

		public static string AssetDirectory = Environment.CurrentDirectory + "\\Assets\\";
		public static bool advancedDebug = true;
	}
}
