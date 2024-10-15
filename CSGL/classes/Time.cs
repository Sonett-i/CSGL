using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL
{
	public class Time
	{
		public enum TimeTypes
		{
			TIME_MILLISECONDS = 0,
			TIME_SECONDS = 1,
			TIME_MINUTES = 2,
			TIME_HOURS = 3,
			TIME_DAYS = 4,
		}

		private static DateTime startTime = DateTime.Now;
		private static DateTime lastTime = startTime;
		
		public static float NextPoll = 0.0f;
		public static float PollInterval = 0.1f;

		public static float deltaTime 
		{
			get 
			{
				return GetDelta();
			}
		}

		public static float time
		{
			get
			{
				return TotalTime();
			}
		}

		private static float GetDelta()
		{
			TimeSpan timeSpan = DateTime.Now - lastTime;
			lastTime = DateTime.Now;

			return (float)timeSpan.TotalMilliseconds;
		}

		private static float TotalTime()
		{
			TimeSpan timeSpan = DateTime.Now - startTime;

			return (float)timeSpan.TotalSeconds;
		}

		public static void Tick()
		{
			lastTime = DateTime.Now;
		}
	}
}
