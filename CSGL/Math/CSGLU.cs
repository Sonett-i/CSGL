using OpenTK.Mathematics;
using System.Reflection;
using System.Text.Json;

#pragma warning disable CS8602

namespace CSGL
{
	public static class CSGLU
	{

		public static object? ConvertJsonElement(JsonElement element, Type targetType)
		{
			if (targetType == typeof(string) && element.ValueKind == JsonValueKind.String)
			{
				return element.GetString();
			}
			else if (targetType == typeof(int) && element.ValueKind == JsonValueKind.Number)
			{
				return element.GetInt32();
			}
			else if (targetType == typeof(float) && element.ValueKind == JsonValueKind.Number)
			{
				return element.GetSingle();
			}
			else if (targetType == typeof(double) && element.ValueKind == JsonValueKind.Number)
			{
				return element.GetDouble();
			}

			return null;
		}

		public static Dictionary<string, Type> GetDerivedTypesDictionary<T>() where T : class
		{
			Type baseType = typeof(T);
			return Assembly.GetAssembly(baseType)
				.GetTypes()
				.Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(baseType)).ToDictionary(t => t.Name, t => t);
		}

		public static List<Type> GetDerivedTypes<T>() where T : class
		{
			Type baseType = typeof(T);
			return Assembly.GetAssembly(baseType).GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(baseType)).ToList();
		}

		public static Vector3 Vector3FromString(string[] data)
		{
			float x = float.Parse(data[1]); // x
			float y = float.Parse(data[2]); // y
			float z = float.Parse(data[3]); // z

			return new Vector3(x, y, z);
		}

		public static Vector2 Vector2FromString(string[] data)
		{
			float x = float.Parse(data[1]);
			float y = float.Parse(data[2]);

			return new Vector2(x, y);
		}

		public static Vector3i Vector3iFromString(string[] data)
		{
			int x = int.Parse(data[0]);
			int y = int.Parse(data[1]);
			int z = int.Parse(data[2]);

			return new Vector3i(x, y, z);
		}

		public static float KiB(float value)
		{
			return value / 1024;
		}

		public static int Random(int min, int max)
		{
			Random r = new Random();

			return r.Next(min, max);
		}

		public static float Random(float min, float max)
		{
			Random r = new Random();

			return (float)(min + r.NextDouble() * (max - min));
		}
	}
}
