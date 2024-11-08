using System;
using OpenTK;
using OpenTK.Mathematics;
using System.Reflection;

namespace CSGL
{
	public class Monobehaviour
	{
		public static Dictionary<string, Type> ObjectTypes; // = Monobehaviour.GetDerivedTypesDictionary<Monobehaviour>();
		public static List<Monobehaviour> Monobehaviours = new List<Monobehaviour>();
		public static List<Monobehaviour> GameObjects = new List<Monobehaviour>();

		public Transform Transform;
		public MeshRenderer MeshRenderer;

		public Monobehaviour()
		{

		}

		public virtual void OnAwake()
		{
			Log.Default($"awake at: {Transform.Position.ToString()}");
			//RenderObject.m_Model = MathU.TRS(Transform);
		}

		public virtual void Start()
		{

		}

		public virtual void Update()
		{

		}

		public virtual void FixedUpdate()
		{

		}

		public virtual void OnRender()
		{

		}

		public static Dictionary<string, Type> GetDerivedTypesDictionary<T>() where T : class
		{
			Type baseType = typeof(T);
			return Assembly.GetAssembly(baseType)
				.GetTypes()
				.Where(t=>t.IsClass && !t.IsAbstract && t.IsSubclassOf(baseType)).ToDictionary(t => t.Name, t=>t);
		}

		public static List<Type> GetDerivedTypes<T>() where T : class
		{
			Type baseType = typeof(T);
			return Assembly.GetAssembly(baseType).GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(baseType)).ToList();
		}
	}
}
