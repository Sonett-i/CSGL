using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logging;

namespace Import
{
	public static class Scan
	{
		public static List<string> scannedFiles = new List<string>();

		public static bool ScanFolders()
		{
			Log.Default("Scanning assets in: " + ManagedDirectories.WorkingDirectory);
			string[] folders = Directory.GetDirectories(ManagedDirectories.WorkingDirectory);

			foreach (string folder in folders)
			{
				Explore(folder);
			}

			Log.Default($"{scannedFiles.Count} files found");

			return true;
		}

		public static string Explore(string path)
		{
			string result = "";

			string[] subDirectory = Directory.GetDirectories(path);

			if (subDirectory.Length > 0)
			{
				for (int i = 0; i < subDirectory.Length; i++)
				{
					result = Explore(subDirectory[i]);
				}
			}

			string[] files = Directory.GetFiles(path);

			foreach (string file in files)
			{
				scannedFiles.Add(file);
			}

			return result;
		}
	}
}
