using Assimp;
using System;
using System.IO;

#pragma warning disable CS8604

namespace CSGL
{
	internal class Resources
	{
		public static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
		public static Dictionary<string, Shader> Shaders = new Dictionary<string, Shader>();
		public static Dictionary<string, MeshFilter> MeshFilters = new Dictionary<string, MeshFilter>();
		public static Dictionary<string, Material> Materials = new Dictionary<string, Material>();

		static List<string> scannedTextures = new List<string>();
		static List<string> scannedShaders = new List<string>();
		static List<string> scannedModels = new List<string>();
		static List<string> scannedMaterials = new List<string>();


		public static void AddTexture(Texture2D texture)
		{
			if (!Textures.ContainsKey(texture.Name))
			{
				Textures[texture.Name] = texture;	
			}
		}

		public static void AddShader(Shader shader)
		{
			if (!Shaders.ContainsKey(shader.Name))
			{
				Shaders[shader.Name] = shader;
			}
		}

		public static void AddMeshFilter(MeshFilter meshfilter)
		{
			if (!MeshFilters.ContainsKey(meshfilter.Name))
			{
				MeshFilters[meshfilter.Name] = meshfilter;
			}
		}

		public static void AddMaterial(Material material)
		{
			if (material == null)
				return;

			if (!Materials.ContainsKey(material.Name))
			{
				Materials[material.Name] = material;
			}
		}

		// Imports resources from the resources directory

		public static void Import()
		{
			// Reset scanned files each import, so  we can add and reload files on the fly (to-do: erase dictionaries)
			scannedTextures = new List<string>();
			scannedShaders = new List<string>();
			scannedModels = new List<string>();
			scannedMaterials = new List<string>();

			// Scan resources directory first
			Scan();

			// Import textures first, then shaders, then we can bind them in our custom material class. Models are imported last.

			ImportTextures(); 
			ImportShaders();
			ImportMaterials();
			ImportModels();
		}

		public static void Scan()
		{
			Log.Default($"Scanning Resources Directory");
			string[] folders = Directory.GetDirectories(EditorConfig.ResourcesDirectory);

			foreach (string folder in folders)
			{
				Explore(folder);
			}

			Log.Default($"Found: \nMaterials: {scannedMaterials.Count}\nShaders: {scannedShaders.Count}\nTextures: {scannedTextures.Count}\nModels {scannedModels.Count}");
		}

		// Recursive Explore function that explores subdirectories
		public static string Explore(string path)
		{
			string result = "";

			string[] subDirectory = Directory.GetDirectories(path);

			if (subDirectory.Length > 0)
			{
				for (int i = 0; i < subDirectory.Length; i++)
				{
					result = Explore(subDirectory[i]); // Recursively explore the subdirectory
				}
			}

			string[] files = Directory.GetFiles(path);

			if (files.Length > 0)
			{
				for (int i = 0; i < files.Length; i++)
				{
					string fileName = Path.GetFileName(files[i]);
					string ext = Path.GetExtension(files[i]);

					if (ext == ".json")
					{
						Asset? asset = Asset.ImportFromJson(files[i]);

						if (asset != null)
						{
							if (asset.Type == Asset.AssetType.ASSET_MATERIAL)
							{
								scannedMaterials.Add(files[i]);
								continue;
							}
						}
					}

					if (ext == ".jpg" || ext == ".png")
					{
						scannedTextures.Add(files[i]);
						continue;
					}

					if (ext == ".obj")
					{
						scannedModels.Add(files[i]);
						continue;
					}

					if (ext == ".vert" || ext == ".frag")
					{
						scannedShaders.Add(files[i]);
						continue;
					}
				}
			}

			return result;
		}

		// Import and Compile Shaders 
		public static void ImportShaders()
		{
			Log.Default("Importing Shaders");
			List<VertexShader> vertexShaders = new List<VertexShader>();
			List<FragmentShader> fragmentShaders = new List<FragmentShader>();

			foreach (string shaderFile in scannedShaders)
			{
				// find vert file first
				string fileName = Path.GetFileNameWithoutExtension(shaderFile); // Path.GetFileName(shaderFile);
				string ext = Path.GetExtension(shaderFile);

				if (ext == ".vert")
				{
					VertexShader vert = new VertexShader(fileName, @File.ReadAllText(shaderFile));
					vertexShaders.Add(vert);
				}

				if (ext == ".frag")
				{
					FragmentShader frag = new FragmentShader(fileName, @File.ReadAllText(shaderFile));
					fragmentShaders.Add(frag);
				}
			}

			// Iterate through vertex shaders and link to fragment.
			foreach (VertexShader vertexShader in vertexShaders)
			{
				foreach (FragmentShader fragmentShader in fragmentShaders)
				{
					if (vertexShader.FileName == fragmentShader.FileName)
					{
						Shader shader = new Shader(vertexShader.FileName, vertexShader, fragmentShader);
						Shaders.Add(shader.Name, shader);
					}
				}
			}

			Log.Default($"Compiled {Shaders.Count} Shaders\n");
		}

		public static void ImportTextures()
		{
			Log.Default("Importing Textures");
			foreach (string textureFile in scannedTextures)
			{
				string ext = Path.GetExtension(textureFile);
				string fileName = Path.GetFileName(textureFile);

				Texture2D texture = new Texture2D(textureFile, fileName);

				//Textures.Add(texture);

				if (texture.Name == "default.png")
					Texture2D.DefaultTexture = texture;

				Textures.Add(texture.Name, texture);

				Log.Default($"Loaded {fileName}: " + texture.ToString());
			}

			Log.Default("Imported " + Textures.Count + " Textures\n");
		}

		public static void ImportModels()
		{
			Log.Default("Importing Models");
			foreach (string modelFile in scannedModels)
			{
				MeshFilter mesh = Model.ImportModel(modelFile);
				MeshFilters.Add(mesh.Name, mesh);
				Log.Default($"Imported {mesh.Name}: " + mesh.ToString());
			}

			Log.Default("Imported " + MeshFilters.Count + " Models");
		}

		public static void ImportMaterials()
		{
			Log.Default("Importing Materials");

			foreach(string materialFile in scannedMaterials)
			{
				Material? mat = Material.LoadFromJson(materialFile);
				if (mat != null)
				{
					Materials.Add(mat.Name, mat);

					if (mat.Name == "default")
						Material.DefaultMaterial = mat;

					Log.Default($"Loaded material: {mat.Name}");
				}
			}
			Log.Default("Imported " + Materials.Count + " materials\n");
		}

		public static void ReloadShaders()
		{
			Log.Default("Reloading shaders");
			

			//ShaderManager.Import();
		}
	}
}
