using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logging;

#pragma warning disable 8604

namespace ContentPipeline
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
				string currentDir = $"\\{new DirectoryInfo(folder).Name}\\";

				if (ManagedDirectories.IgnoredDirectories.ContainsKey(currentDir))
				{
					continue;
				}

				Explore(folder);
			}

			return true;
		}

		public static string Explore(string path)
		{
			string result = "";
			string currentDir = $"\\{new DirectoryInfo(Path.GetDirectoryName(path)).Name}\\";
			Log.Default(path);

			if (ManagedDirectories.IgnoredDirectories.ContainsKey(currentDir))
			{
				return "";
			}

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
				string ext = Path.GetExtension(file);

				if (ManagedFormats.Extensions.ContainsKey(ext))
				{
					scannedFiles.Add(file);
				}
			}

			return result;
		}
	}
}
