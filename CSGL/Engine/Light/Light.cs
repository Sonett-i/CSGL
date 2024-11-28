﻿using System;
using OpenTK.Mathematics;
using ContentPipeline;
using OpenTK.Graphics.OpenGL;
using CSGL.Assets;
using CSGL.Graphics;
using SharedLibrary;

public enum LightType
{
	POINT,
	DIRECTIONAL,
	SPOTLIGHT
}

namespace CSGL.Engine
{
	public class Light : Entity
	{
		public Color4 Colour = Color4.White;
		public float intensity = 1.0f;

		public Vector3 ambient = Vector3.One * 1f;
		public Vector3 diffuse = Vector3.One * 1f;
		public Vector3 specular = Vector3.One * 1f;

		public LightType lightType = LightType.POINT;

		public Light(Color4 colour, float ambient, float intensity, float diffuse, float specular, LightType lightType = LightType.POINT) : base("Light")
		{
			base.EntityType = EntityType.Light;
			this.lightType = lightType;

			this.Colour = colour;
			this.intensity = intensity;

			this.ambient = Vector3.One * ambient;
			this.diffuse = Vector3.One * diffuse;
			this.specular = Vector3.One * specular;

			List<Texture> texList = new List<Texture>();

			Texture tex = new Texture("default.png", TextureType.DIFFUSE, TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.UnsignedByte);

			texList.Add(tex);

			this.model = new Model(Manifest.GetAsset<ModelAsset>("cube.obj"));
			this.model.shader = ShaderManager.Shaders["light.shader"];
		}

		public override void Start()
		{

			base.Start();
		}

		public override void Update()
		{

			base.Update();
		}

		public override void Render()
		{
			base.Render();
		}
	}
}