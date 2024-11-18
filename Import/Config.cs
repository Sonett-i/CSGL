using Import.Extensions;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Import
{
	public static class Config
	{
		public static INI? Import()
		{
			if (File.Exists(ManagedFiles.EngineConfig))
			{
				INI configIni = new INI(ManagedFiles.EngineConfig);

				return configIni;
			}

			return null;
		}
	}
}
