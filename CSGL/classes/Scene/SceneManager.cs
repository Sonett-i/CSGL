using System;
using System.Text.Json;
using System.IO;
using OpenTK.Mathematics;
using static CSGL.Asset;

namespace CSGL
{
	public class SceneManager
	{
		public static List<Scene> Scenes = new List<Scene>();
		public static Scene CurrentScene = new Scene("Default");

		public static bool isReloading = false;

		public static Scene? LoadScene(string sceneName)
		{
			foreach (Scene scene in Scenes)
			{
				if (scene.Name == sceneName)
					return scene;
			}

			return null;
		}

		public static void ReloadScene(string filepath)
		{
			Log.Default("Reloading current scene");
			Camera cache = Camera.main;

			Camera.main = new Camera(new Vector3(0.0f, 0.0f, -8.0f), ProjectionType.PROJECTION_PROJECTION, 0.1f, 100f, 45f);

			SceneManager.CurrentScene.Unload();

			isReloading = true;
			if (File.Exists(filepath))
			{
				using (JsonDocument document = JsonDocument.Parse(File.ReadAllText(filepath)))
				{
					JsonElement root = document.RootElement;
					string assetType = root.GetProperty("assetType").ToString();
					string name = root.GetProperty("name").ToString();

					if (assetType == "scene")
					{
						Scene scene = SceneManager.ImportFromJson(root, filepath);

						if (scene != null)
						{
							SceneManager.CurrentScene = scene;
							//SceneManager.Scenes.Add(scene);
						}
					}
				}
			}
			isReloading = false;

			Camera.main = cache;
			Log.Default("Done");
		}

		public static Scene ImportFromJson(JsonElement root, string filepath)
		{
			string sceneName = root.GetProperty("name").GetString() ?? "scene";

			Scene scene = new Scene(sceneName);

			scene.FilePath = filepath;

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


					Transform transform = new Transform(new Vector3(pX, pY, pZ), MathU.Euler(rX, rY, rZ), new Vector3(sX, sY, sZ));

					JsonElement components = gobject.GetProperty("components");

					string componentType = components.GetProperty("type").GetString() ?? "renderobject";
					string modelName = components.GetProperty("mesh").GetString() ?? "cube";
					string material = components.GetProperty("material").GetString() ?? "default";

					/*
					if (componentType == "renderobject")
					{
						//Model model = ModelManager.LoadModel(modelName);
						//Material mat = MaterialManager.GetMaterial(material);

						RenderObject renderobject = new RenderObject(model, mat, OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);// ModelManager.LoadModel(modelName).renderObject;

						renderobject.Material = mat;

						if (model != null && mat != null)
						{
							GameObject go = null;// new GameObject(transform, renderobject);
							//GameObject textureTest = new GameObject(new Transform(new Vector3(0, 0, 0), MathU.Euler(0, 0, 0), Vector3.One), cube);
							if (go != null)
							{
								scene.AddObjectToScene(go);
							}
						}
					}
					*/

					if (componentType == "cubemap")
					{
						scene.cubemap = new Cubemap();
					}
				}
			}

			Log.Default($"Imported scene {scene.Name} with {scene.sceneGameObjects.Count} objects");

			return scene;
		}
	}
}