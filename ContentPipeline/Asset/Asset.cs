using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ContentPipeline
{
	public abstract class Asset
	{
		public Guid ID { get; private set; }

		public string Name = "";
		public string FilePath = "";
		public string ext = "";
		public AssetType Type = AssetType.ASSET_BINARY;

		public Asset()
		{
			ID = Guid.NewGuid();
		}

		public Asset(string filePath)
		{

		}

		public virtual void SaveToFile()
		{

		}

		public virtual void LoadFromFile(string filePath)
		{
			if (FilePath != "")
			{
				this.Name = Path.GetFileName(FilePath);
				this.ext = Path.GetExtension(FilePath);
			}

			if (File.Exists(filePath + ".json"))
			{

			}
		}
	}
}
