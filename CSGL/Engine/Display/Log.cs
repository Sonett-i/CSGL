using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL
{
	public static class Log
	{
		public static List<string> logMessages = new List<string>();
		public static List<string> logErrors = new List<string>();

		public static void Default(string message) 
		{ 
			Console.WriteLine(message);
			logMessages.Add(message);
		}

		public static void Error(string message)
		{
			Console.WriteLine($"[CSGL ERROR]: {message}");
			logErrors.Add(message);
			// to-do write log to file
		}

		public static void Advanced(string message) 
		{
			if (EditorConfig.advancedDebug)
				Console.WriteLine(message);
		}
	}
}
