using Import.Extensions;
using Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import
{
	public class ManagedFormats
	{
		// formats that CSGL manages

		public static Dictionary<string, bool> Extensions = new Dictionary<string, bool>();

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
							Extensions.Add(kvp.Key, bool.Parse(kvp.Value));
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
