using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentPipeline
{
	public enum AssetType
	{
		ASSET_TEXT,
		ASSET_TEXTURE,
		ASSET_MODEL,
		ASSET_SHADER,
		ASSET_MATERIAL,
		ASSET_BINARY
	}

	public class AssetManager
	{
		//public static Dictionary<AssetType, Dictionary<Guid, Asset>> Assets = new Dictionary<AssetType, Dictionary<Guid, Asset>>();

		public static AssetType GetAssetType(string asset)
		{
			if (Enum.TryParse(asset, true, out AssetType type)) 
				return type;
			else
			{
				Logging.Log.Error("Invalid asset type");
				throw new Exception("Invalid asset type");
			}
		}

		public static AssetType GetAssetFromType(Type type)
		{
			return type switch
			{
				_ when type == typeof(Model) => AssetType.ASSET_MODEL,
				_ when type == typeof(Material) => AssetType.ASSET_MATERIAL,
				_ when type == typeof(Texture2D) => AssetType.ASSET_TEXTURE,
				_ => throw new Exception($"Unknown asset type")
			};
		}

		public static Asset? GetAssetByName<T>(string assetName) where T : Type
		{

			return null;
		}

		public static Asset? Import(string filePath)
		{
			string fileName = Path.GetFileName(filePath);
			string ext = Path.GetExtension(filePath);
			AssetType assetType = ManagedFormats.Extensions[ext];

			try
			{
				if (ManagedFormats.FactoryMethods.TryGetValue(assetType, out Func<string, Asset>? factoryMethod))
				{
					return factoryMethod(filePath);
				}
				else
				{
					throw new NotSupportedException($"No factory method for {assetType}");
				}
			}
			catch (Exception ex)
			{
				Log.Error($"{ex.Message}");

				return null;
			}
		}
	}
}
