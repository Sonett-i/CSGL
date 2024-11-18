using System;
using System.IO;

namespace Logging
{
	public class Log
	{
		public static void Default(string message)
		{
			Console.WriteLine(message);
		}

		public static void Error(string message)
		{
			Console.WriteLine(message);
		}

		public static void Fatal(string message)
		{

		}

		public static void Init()
		{
			// Erase logging files and start writing
		}
	}
}
