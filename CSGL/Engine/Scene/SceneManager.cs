using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL
{
	public class SceneManager
	{
		public static Dictionary<int, Scene> scenes = new Dictionary<int, Scene>();

		public static Scene ActiveScene = null!;
		public static int AddScene(Scene scene)
		{
			if (!scenes.ContainsKey(scene.sceneID))
			{
				scene.sceneID = scenes.Count;
				scenes[scene.sceneID] = scene;
			}
			return scene.sceneID;
		}
	}
}
