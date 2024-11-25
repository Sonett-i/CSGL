using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL.Engine
{
	public static class Time
	{
		public static float fixedDeltaTime = 0.02f;
		public static float accumulatedTime = 0.0f;

		public static float time = 0.0f;
		public static float deltaTime;
		private static float lastTime = 0.0f;

		private static float PollTime = 0.0f;
		public static float PollInterval = 1.0f;

		public static float timeInterval;

		public static float InterpolationFactor = 0.0f;

		public static void Update(FrameEventArgs e)
		{
			Time.time += (float) e.Time;
			Time.deltaTime = Time.time - Time.lastTime;
			Time.lastTime = Time.time;

			PollTime = Time.time;

			if (PollTime > PollInterval)
			{
				Poll();
				PollTime = 0;
			}
		}

		public static void Poll()
		{
			MainWindow.Instance.Title = $"{EngineConfig.Name}:{EngineConfig.Version}: {Time.deltaTime}";
		}
	}
}
