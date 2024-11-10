using System;
using OpenTK.Mathematics;

namespace CSGL
{
	public class Satellite : Monobehaviour
	{
		MeshFilter meshFilter = null!;
		MeshRenderer meshRenderer = null!;

		Moon orbitMoon = null!;

		float orbitRadius = 165f;
		float orbitSpeed = 0.01f;
		float orbitAngle = 0.0f;

		public override void OnAwake()
		{
			meshFilter = this.GetComponent<MeshFilter>();

			meshFilter.Set(Resources.MeshFilters["Asteroid.obj"]);

			meshRenderer = this.GetComponent<MeshRenderer>();
			this.meshRenderer.Create(meshFilter, Resources.Materials["Asteroid"]);

			base.OnAwake();
		}

		void RandomizeOrbit()
		{
			orbitRadius = CSGLU.Random(50, 150);
			orbitSpeed = CSGLU.Random(0.05f, 0.1f);
			orbitAngle = CSGLU.Random(-90.0f, 90.0f);
		}

		public void SetOrbit(float dst, float speed, float angle)
		{
			this.orbitRadius = dst;
			this.orbitSpeed = speed;
			this.orbitAngle = angle;
		}

		public override void Start()
		{
			//RandomizeOrbit();
			
			base.Start();
		}

		void Orbit()
		{
			if (orbitMoon != null)
			{
				orbitAngle += orbitSpeed * Time.deltaTime;

				float x = orbitMoon.Transform.Position.X + orbitRadius * MathF.Cos(orbitAngle);
				float y = orbitMoon.Transform.Position.Y + orbitRadius * MathF.Sin(orbitAngle);
				float z = orbitMoon.Transform.Position.Z + orbitRadius * MathF.Sin(orbitAngle);

				this.Transform.WorldPosition = new Vector3(x, y, z);
				this.Transform.LocalRotation = Quaternion.FromEulerAngles(MathU.Rad(x), 0, MathU.Rad(z)) * 0.01f;// * Time.deltaTime;
			}
		}

		public override void Update()
		{
			Orbit();
			
			base.Update();
		}

		public void SetMoon(Moon orbitMoon)
		{
			this.orbitMoon = orbitMoon;
		}
	}
}
