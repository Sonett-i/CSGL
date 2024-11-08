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
		
		// Files
		public static string ResourcesDirectory = Environment.CurrentDirectory + "\\Resources\\";
		public static string AssetDirectory = Environment.CurrentDirectory + "\\Assets\\";

		// Debugging
		public static bool advancedDebug = true;
	}
}
