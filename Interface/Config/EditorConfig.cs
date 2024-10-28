using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
	public static class EditorConfig
	{
		public static string ShaderDirectory = Environment.CurrentDirectory + "\\Resources\\Shaders\\";
		public static string ModelDirectory = Environment.CurrentDirectory + "\\Resources\\Models\\";
		public static string AssetDirectory = Environment.CurrentDirectory + "\\Assets\\";
		public static bool advancedDebug = false;
	}
}
