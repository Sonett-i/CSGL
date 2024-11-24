using ContentPipeline.Extensions;
using ContentPipeline.Utilities;
using Logging;
using System;
using System.IO;


namespace ContentPipeline
{
	public class ShaderAsset : Asset
	{
		public string VertexShader = null!;
		public string FragmentShader = null!;

		public string VertexShaderCode = null!;
		public string FragmentShaderCode = null!;

		public string VertexShaderFile
		{
			get
			{
				return Path.GetFileName(this.VertexShader);
			}
		}

		public string FragmetnShaderFile
		{
			get
			{
				return Path.GetFileName(this.FragmentShader);
			}
		}

		public ShaderAsset()
		{
			base.Type = AssetType.ASSET_SHADER;
		}

		public ShaderAsset(string filePath)
		{
			base.Type = AssetType.ASSET_SHADER;
		}

		public void InitializeFields(string filePath)
		{
			if (filePath == "")
				return;

			this.FilePath = filePath;
			this.Name = Path.GetFileNameWithoutExtension(filePath) + ".shader";
			this.ext = Path.GetExtension(filePath);

			switch (this.ext)
			{
				case ".vert":
					this.VertexShader = this.FilePath;
					getFragmentShader();
					break;
				case ".frag":
					this.FragmentShader = this.FilePath;
					getVertexShader();
					break;
			}

			this.VertexShaderCode = File.ReadAllText(this.VertexShader);
			this.FragmentShaderCode = File.ReadAllText(this.FragmentShader);

			Log.Info($"{this.Name} (Vertex shader: {this.VertexShaderFile}, Fragment Shader: {this.FragmetnShaderFile}) loaded {this.ID}");
		}

		private void getFragmentShader()
		{
			string fragmentPath = this.FilePath.Replace(ext, ".frag");

			if (File.Exists(fragmentPath))
			{
				Scan.scannedFiles.Remove(fragmentPath);
				this.FragmentShader = fragmentPath;
			}
		}

		private void getVertexShader()
		{
			string vertexPath = this.FilePath.Replace(ext, ".vert");
			if (File.Exists(vertexPath))
			{
				Scan.scannedFiles.Remove(vertexPath);
				this.VertexShader = vertexPath;
			}
		}
	}
}
