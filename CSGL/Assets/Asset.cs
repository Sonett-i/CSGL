using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
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

		public static Monobehaviour ImportObject(string file)
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


				if (root.TryGetProperty("components", out JsonElement componentsElement))
				{
					components = GetComponents(componentsElement);
				}
			}

			Type classType = Monobehaviour.ObjectTypes[objectClass];

			if (classType == null || !typeof(Monobehaviour).IsAssignableFrom(classType)) 
			{
				throw new Exception($"{objectClass} not found, or is not a valid monobehavior");
			}

			Monobehaviour gameobject = (Monobehaviour)Activator.CreateInstance(classType);


			//GameObject go = new GameObject()

			return null;
		}

		public static List<Component> GetComponents(JsonElement componentsElement)
		{
			List<Component> list = new List<Component>();

			foreach (JsonProperty componentProperty in componentsElement.EnumerateObject())
			{
				string componentName = componentProperty.Name;

				JsonElement componentConfig = componentProperty.Value;

				if (Component.ComponentTypes.TryGetValue(componentName, out Type componentType))
				{
					Component component = (Component)Activator.CreateInstance(componentType);

					component.Instance(new object[] {"test", "abc"});

					foreach (JsonProperty property in componentConfig.EnumerateObject())
					{
						string propertyName = property.Name;
						JsonElement propertyValue = property.Value;

						PropertyInfo propInfo = componentType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
						if (propInfo != null && propInfo.CanWrite)
						{
							object value = Convert.ChangeType(propertyValue.ToString(), propInfo.PropertyType);
							propInfo.SetValue(component, value);
						}
					}

					list.Add(component);
				}
				//objectComponents.Add(objectComponent.GetString() ?? "null");
			}


			return list;
		}

		public static object CreateInstance(string typeName, params object[] args)
		{
			if (Monobehaviour.ObjectTypes.TryGetValue(typeName, out Type type))
			{
				return Activator.CreateInstance(type, args); // Pass the parameters here
			}

			throw new ArgumentException($"Type '{typeName}' not found.");
		}

		public static Scene ImportScene(string file)
		{
			return null;
		}
	}
}
