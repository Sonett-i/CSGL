using System;
using System.Text.Json;
using CSGL;

namespace Interface
{
	internal class LoadConfig
	{
		public static void ImportFromJson()
		{
			if (File.Exists(EditorConfig.ConfigDirectory + "WindowConfig.json"))
			{
				string jsonString = File.ReadAllText(EditorConfig.ConfigDirectory + "WindowConfig.json");

				using (JsonDocument document = JsonDocument.Parse(jsonString))
				{
					JsonElement root = document.RootElement;

					WindowConfig.Width = root.GetProperty("Width").GetInt32();
					WindowConfig.Height = root.GetProperty("Height").GetInt32();
					WindowConfig.Name = root.GetProperty("Title").GetString() ?? "OpenTK";
					WindowConfig.VSyncMode = root.GetProperty("VSync").GetBoolean() ? OpenTK.Windowing.Common.VSyncMode.On : OpenTK.Windowing.Common.VSyncMode.Off;

					WindowConfig.StickyMouse = root.GetProperty("StickyMouse").GetBoolean();
					WindowConfig.CursorVisible = root.GetProperty("CursorVisible").GetBoolean();
				}
			}
		}
	}
}
