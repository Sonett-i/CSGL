using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace CSGL
{
	public class Asset
	{
		public static Dictionary<string, AssetType> AssetTypes = new Dictionary<string, AssetType>()
		{
			["model"] = AssetType.ASSET_MODEL,
			["material"] = AssetType.ASSET_MATERIAL,
			["scene"] = AssetType.ASSET_SCENE,
			["gameobject"] = AssetType.ASSET_GAMEOBJECT,

		};

		public enum AssetType
		{
			ASSET_MODEL,
			ASSET_TXT,
			ASSET_MATERIAL,
			ASSET_GAMEOBJECT,
			ASSET_SCENE
		}

		public string Name;
		public string filePath;
		public string extension;
		public AssetType Type;
		public JsonDocument Contents;

		public Asset(string name, string filePath, string extension)
		{
			Name = name;
			this.filePath = filePath;
			this.extension = extension;
		}

		public static Asset? ImportFromJson(string filePath)
		{
			if (Path.GetExtension(filePath) != ".json")
			{
				return null;
			}

			Asset asset = new Asset("null", "null", "null");

			string jsonString = File.ReadAllText(filePath);

			using (JsonDocument document = JsonDocument.Parse(jsonString))
			{
				JsonElement root = document.RootElement;
				string assetType = root.GetProperty("assetType").ToString();
				string name = root.GetProperty("name").ToString();

				asset.Name = name;
				asset.filePath = filePath;
				asset.Type = AssetTypes[assetType];
			}

			return asset;
		}

		public static GameObject FromAsset(Asset asset)
		{
			//GameObject go = new GameObject()

			return null;
		}
	}
}
