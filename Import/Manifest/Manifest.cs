using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import
{
	public class Manifest
	{
		public static Dictionary<int, Asset> FileManifest = new Dictionary<int, Asset>();
		public static void UpdateFileManifest()
		{
			 Scan.ScanFolders();

			for (int i = 0; i < Scan.scannedFiles.Count; i++)
			{

			}
			// scan all files first


			// then add to File Manifest
		}
	}
}
