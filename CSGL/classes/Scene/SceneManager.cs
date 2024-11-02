using System;
using System.Text.Json;
using System.IO;
using OpenTK.Mathematics;

namespace CSGL
{
	public class SceneManager
	{
		public static List<Scene> Scenes = new List<Scene>();

		public static Scene? LoadScene(string sceneName)
		{
			foreach (Scene scene in Scenes)
			{
				if (scene.Name == sceneName)
					return scene;
			}

			return null;
		}

		public static void ImportFromJson(JsonElement root)
		{
			string sceneName = root.GetProperty("name").GetString() ?? "scene";

			Scene scene = new Scene(sceneName);
			List<GameObject> objects = new List<GameObject>();

			if (root.TryGetProperty("gameobjects", out JsonElement gameobjectElement))
			{
				foreach (JsonElement gameobject in gameobjectElement.EnumerateArray())
				{
					JsonElement gobject = gameobject.GetProperty("gobject");
					
					string name = gobject.GetProperty("name").GetString() ?? "gameobject";

					JsonElement position = gobject.GetProperty("position");
					JsonElement rotation = gobject.GetProperty("rotation");
					JsonElement scale = gobject.GetProperty("scale");

					float pX = position.GetProperty("x").GetSingle();
					float pY = position.GetProperty("y").GetSingle();
					float pZ = position.GetProperty("z").GetSingle();

					float rX = rotation.GetProperty("x").GetSingle();
					float rY = rotation.GetProperty("y").GetSingle();
					float rZ = rotation.GetProperty("z").GetSingle();

					float sX = scale.GetProperty("x").GetSingle();
					float sY = scale.GetProperty("y").GetSingle();
					float sZ = scale.GetProperty("z").GetSingle();


					Transform transform = new Transform(new Vector3(pX, pY, pZ), MathU.Euler(rX, rY, rZ), Vector3.One);

					JsonElement components = gobject.GetProperty("components");

					string componentType = components.GetProperty("type").GetString() ?? "RenderObject";
					string modelName = components.GetProperty("mesh").GetString() ?? "cube";
					string material = components.GetProperty("material").GetString() ?? "default";

					if (componentType == "renderobject")
					{
						Model model = ModelManager.LoadModel(modelName);
						Material mat = MaterialManager.GetMaterial(material);

						RenderObject renderobject = ModelManager.LoadModel(modelName).renderObject;

						renderobject.SetMaterial(mat);

						if (model != null && mat != null)
						{
							GameObject go = new GameObject(transform, renderobject);
							//GameObject textureTest = new GameObject(new Transform(new Vector3(0, 0, 0), MathU.Euler(0, 0, 0), Vector3.One), cube);
							if (go != null)
							{
								scene.AddObjectToScene(go);
							}
						}
					}

					if (componentType == "cubemap")
					{
						scene.Cubemap = new Cubemap();
					}
				}
			}

			Log.Default($"Imported scene {scene.Name} with {scene.sceneGameObjects.Count} objects");

			SceneManager.Scenes.Add(scene);
		}
	}
}
