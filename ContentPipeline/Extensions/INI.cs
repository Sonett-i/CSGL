using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace ContentPipeline.Extensions
{
	public class INI
	{
		string Path = "";
		string? EXE = Assembly.GetExecutingAssembly().GetName().Name;

		public Dictionary<string, Dictionary<string, string>> Contents = new Dictionary<string, Dictionary<string, string>>();
		public INI(string filePath)
		{
			this.Path = filePath;

			string[] lines = File.ReadAllLines(filePath);

			string currentHeader = string.Empty;

			for (int i = 0; i < lines.Length; i++)
			{
				string trimmedLine = lines[i].Trim();

				if (string.IsNullOrEmpty(trimmedLine) || lines[i][0] == ';') // Comment
					continue;

				if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
				{
					currentHeader = trimmedLine.Trim('[', ']');
					if (!Contents.ContainsKey(currentHeader))
					{
						Contents[currentHeader] = new Dictionary<string, string>();
					}
				}
				else if (!string.IsNullOrEmpty(currentHeader))
				{
					string[] keyValue = trimmedLine.Split(new[] { '=' }, 2);

					if (keyValue.Length == 2)
					{
						string key = keyValue[0].Trim();
						string value = keyValue[1].Trim();

						Contents[currentHeader][key] = value;
					}
				}
			}
		}
	}
}
