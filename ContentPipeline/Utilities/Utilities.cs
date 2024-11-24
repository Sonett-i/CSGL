using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentPipeline.Utilities
{
	internal class Util
	{
		public static string KiB(float input)
		{
			return $"{(input / 1024)} KiB";
		}
	}
}
