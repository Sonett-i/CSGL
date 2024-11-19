using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentPipeline
{
	public static class ManagedDirectories
	{
		public static string WorkingDirectory = Environment.CurrentDirectory;

		// Special case
		public static string ConfigDirectory = WorkingDirectory + "\\Config\\";

		public static string LogDirectory = WorkingDirectory + "\\Logs\\";

	}
}
