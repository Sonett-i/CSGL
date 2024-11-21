using System;
using System.IO;
using System.Diagnostics;

namespace Logging
{
	public class Log
	{
		public enum LogType
		{
			LOG_DEFAULT,
			LOG_ERROR,
			LOG_FATAL,
			LOG_INFO,
			LOG_GL
		}

		static string directory = "";
		static string currentRun = "";

		static Dictionary<LogType, string> logFiles = new Dictionary<LogType, string>();



		public static void Default(string message)
		{
			Console.WriteLine(message);
			LogToFile(LogType.LOG_DEFAULT, message);
		}

		public static void Error(string message)
		{
			Console.WriteLine($"[Error]: {message}");
			LogToFile(LogType.LOG_ERROR, message);
		}

		public static void Fatal(string message)
		{
			LogToFile(LogType.LOG_FATAL, message);
		}

		public static void Info(string message)
		{
			Console.WriteLine($"{message}");
			LogToFile(LogType.LOG_INFO, message);
		}

		public static void GL(string message)
		{
			Console.WriteLine($"[OpenGL]: {message}");
			LogToFile(Log.LogType.LOG_GL, message);
		}

		public static void LogToFile(LogType logType, string message)
		{
			if (logFiles.ContainsKey(logType))
			{
				string filePath  = logFiles[logType];
				File.AppendAllText(filePath, $"{DateTime.Now:dd-mm-yy-HH:mm:ss} - {message}\n");
			}
		}

		// Initialize logging directories and files
		public static void Init(string logDirectory)
		{
			currentRun = DateTime.Now.ToString("ddmmyy_HHmm");

			directory = logDirectory;

			if (!Directory.Exists(logDirectory))
				Directory.CreateDirectory(logDirectory);

			if (!Directory.Exists(logDirectory + currentRun))
				Directory.CreateDirectory(logDirectory + currentRun);
			
			logFiles[LogType.LOG_DEFAULT] = CreateLogFile("Default");
			logFiles[LogType.LOG_ERROR] = CreateLogFile("Error");
			logFiles[LogType.LOG_FATAL] = CreateLogFile("Fatal");
			logFiles[LogType.LOG_INFO] = CreateLogFile("Info");
			logFiles[LogType.LOG_GL] = CreateLogFile("OpenGL");
		}

		public static string CreateLogFile(string logTypeName)
		{
			string filePath = Path.Combine(directory + currentRun, $"{currentRun}_{logTypeName}.log");

			using (File.Create(filePath)) { }
			return filePath;
		}
	}
}
