using ContentPipeline.Extensions;
using Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentPipeline
{
	public class ManagedFormats
	{
		// formats that CSGL manages

		public static Dictionary<string, AssetType> Extensions = new Dictionary<string, AssetType>();

		public static Dictionary<AssetType, Func<string, Asset>> FactoryMethods = new Dictionary<AssetType, Func<string, Asset>>()
		{
			{ AssetType.ASSET_MODEL, filePath => { Model model = new Model(filePath); model.InitializeFields(filePath); return model; } },
			{ AssetType.ASSET_SHADER, filePath => { ShaderAsset shader = new ShaderAsset(filePath); shader.InitializeFields(filePath); return shader; } },
			{ AssetType.ASSET_TEXTURE, filePath => { TextureAsset texture = new TextureAsset(filePath); texture.InitializeFields(filePath); return texture; } },
			//{AssetType.ASSET_TEXTURE, filePath => new T { FilePath = filePath } },
		};

		

		public static void Configure(INI Config)
		{
			try
			{
				if (Config.Contents.ContainsKey("FileFormats"))
				{
					foreach (KeyValuePair<string, string> kvp in Config.Contents["FileFormats"])
					{
						if (!Extensions.ContainsKey(kvp.Key))
						{
							AssetType type = AssetManager.GetAssetType(kvp.Value);
							Extensions.Add(kvp.Key, type);
						}
					}
				}

				if (Config.Contents.ContainsKey("IgnoredDirectories"))
				{
					foreach (KeyValuePair<string, string> kvp in Config.Contents["IgnoredDirectories"])
					{
						if (!ManagedDirectories.IgnoredDirectories.ContainsKey(kvp.Key))
						{
							ManagedDirectories.IgnoredDirectories.Add(kvp.Key, bool.Parse(kvp.Value));
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message);
			}
		}
	}
}
