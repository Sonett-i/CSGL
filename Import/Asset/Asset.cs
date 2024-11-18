using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import
{
	public enum AssetType
	{
		ASSET_TEXT,
		ASSET_IMAGE,
		ASSET_MODEL,
		ASSET_SHADER,
		ASSET_MATERIAL,
		ASSET_BINARY
	}

	public class Asset
	{
		public int ID = -1;

		public string? Name = "";
		public string FilePath = "";
		public string ext = "";
		public AssetType Type = AssetType.ASSET_BINARY;

		public Asset()
		{

		}


		public static Asset Import(string filePath)
		{
			Asset asset = new Asset();

			return asset;
		}
	}
}
