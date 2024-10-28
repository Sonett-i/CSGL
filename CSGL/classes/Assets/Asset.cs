using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL
{
	public class Asset
	{
		public enum AssetType
		{
			ASSET_MODEL,
			ASSET_TXT,
			ASSET_SHADER,
		}

		public string Name;
		public string filePath;
		public string extension;

		public Asset(string name, string filePath, string extension)
		{
			Name = name;
			this.filePath = filePath;
			this.extension = extension;
		}

		public static Asset Import()
		{
			return null;
		}
	}
}
