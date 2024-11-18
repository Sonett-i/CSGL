using System;
using System.IO;

namespace Log
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
	}
}
