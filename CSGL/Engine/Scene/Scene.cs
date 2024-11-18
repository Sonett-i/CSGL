using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logging;

namespace CSGL
{
	public class Scene
	{
		public int sceneID = -1;
		public readonly string Name = "";
		public Scene(string name) 
		{ 
			this.Name = name; 
			this.sceneID = SceneManager.AddScene(this);
			Awake(); 
		}

		public virtual void Awake()
		{
			Log.Default(Name + " scene awake");
		}

		public virtual void Start()
		{

		}

		public virtual void Update()
		{
			
		}

		public virtual void Render()
		{

		}
	}
}
