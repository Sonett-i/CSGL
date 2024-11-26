using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logging;

namespace CSGL.Engine
{
	public class Error
	{
		public static void Fatal(string message)
		{
			Log.Fatal(message);
			Environment.Exit(0);
		}
	}
}
