using System.Text.Json;

#pragma warning disable CS8602

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
		public JsonDocument? Contents;

		public Asset(string name, string filePath, string extension)
		{
			Name = name;
			this.filePath = filePath;
			this.extension = extension;
		}

		public static Asset? ImportFromJson(string filePath)
		{
			if (Path.GetExtension(filePath) != ".json")
				return null;

			Asset asset = new Asset("null", "null", "null");

			string jsonString = File.ReadAllText(filePath);

			using (JsonDocument document = JsonDocument.Parse(jsonString))
			{
				JsonElement root = document.RootElement;
				string assetType = root.GetProperty("assetType").ToString().ToLower();
				string name = root.GetProperty("name").ToString();

				asset.Name = name;
				asset.filePath = filePath;
				asset.Type = AssetTypes[assetType];
			}

			return asset;
		}

		public static Monobehaviour? ImportObject(string file)
		{
			string jsonString = File.ReadAllText(file);

			string objectName = "";
			string objectModel = "";
			string objectMaterial = "";
			string objectClass = "";
			List<string> objectComponents = new List<string>();
			List<Component> components = new List<Component>();

			using (JsonDocument document = JsonDocument.Parse(jsonString))
			{
				JsonElement root = document.RootElement;

				objectName = root.GetProperty("name").ToString();
				objectModel = root.GetProperty("model").ToString();
				objectMaterial = root.GetProperty("material").ToString();
				objectClass = root.GetProperty("objectclass").ToString();

				Type classType = Monobehaviour.ObjectTypes[objectClass];

				if (classType == null || !typeof(Monobehaviour).IsAssignableFrom(classType))
				{
					throw new Exception($"{objectClass} not found, or is not a valid monobehavior");
				}

				Monobehaviour? gameobject = Activator.CreateInstance(classType) as Monobehaviour;

				if (root.TryGetProperty("components", out JsonElement componentsElement))
				{
					foreach (JsonProperty componentProperty in componentsElement.EnumerateObject())
					{
						string componentName = componentProperty.Name;

						JsonElement componentConfig = componentProperty.Value;

						if (Component.ComponentTypes.TryGetValue(componentName, out Type? componentType))
						{
							Dictionary<string, JsonElement> jsonElements = new Dictionary<string, JsonElement>();
							Component? component = Activator.CreateInstance(componentType) as Component;

							foreach (JsonProperty property in componentConfig.EnumerateObject())
							{
								string propertyName = property.Name;
								JsonElement propertyValue = property.Value;

								jsonElements.Add(propertyName, property.Value);

							}

							if (gameobject != null)
							{
								component.Instance(gameobject, jsonElements);
								gameobject.AddComponent(component);
							}
						}
					}
				}

				if (gameobject != null)
				{
					AssetManager.Monobehaviours.Add(objectName, gameobject);
				}
			}

			//GameObject go = new GameObject()

			return null;
		}

		public static object? CreateInstance(string typeName, params object[] args)
		{
			if (Monobehaviour.ObjectTypes.TryGetValue(typeName, out var type))
			{
				return Activator.CreateInstance(type, args); // Pass the parameters here
			}

			throw new ArgumentException($"Type '{typeName}' not found.");
		}

		public static Scene? ImportScene(string file)
		{
			return null;
		}
	}
}
