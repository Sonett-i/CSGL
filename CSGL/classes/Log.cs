using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL
{
	public static class Log
	{
		public static void Default(string message) 
		{ 
			Console.WriteLine(message);
		}

		public static void Advanced(string message) 
		{
			if (EditorConfig.advancedDebug)
				Console.WriteLine(message);
		}
	}
}
