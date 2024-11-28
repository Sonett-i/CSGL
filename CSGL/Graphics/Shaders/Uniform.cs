using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CSGL.Graphics
{
	public class Uniform
	{
		public string Name { get; private set; }
		public int Location { get; private set; }
		public ActiveUniformType Type { get; private set; }
		public int Size { get; private set; }

		public Uniform(string name, int location, ActiveUniformType type, int size)
		{
			Name = name;
			Location = location;
			Type = type;
			Size = size;
		}

		public void SetValue(float value)
		{
			GL.Uniform1(this.Location, value);
		}

		public void SetValue(Vector2 value)
		{
			GL.Uniform2(this.Location, value.X, value.Y);
		}

		public void SetValue(Vector3 value)
		{
			GL.Uniform3(this.Location, value.X, value.Y, value.Z);
		}

		public void SetValue(Vector4 value)
		{
			GL.Uniform4(this.Location, value.X, value.Y, value.Z, value.W);
		}

		public void SetValue(Matrix4 value)
		{
			GL.UniformMatrix4(this.Location, false, ref value);
		}

		public void SetValue(Color4 value)
		{
			SetValue(new Vector4(value.R, value.G, value.B, value.A));
		}
	}
}
