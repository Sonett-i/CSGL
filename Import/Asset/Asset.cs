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

	internal class Asset
	{
		public AssetType Type = AssetType.ASSET_BINARY;
		public string? Name = "";
		public int ID = -1;

		public Asset()
		{

		}
	}
}
