using System;

namespace CSGL
{
	public struct Colour
	{
		float r;
		float g;
		float b;
		float a;

		public Colour(float r, float g, float b) 
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = 1;
		}

		public Colour(float r, float g, float b, float a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		public override string ToString()
		{
			return $"({r}, {g}, {b}, {a})";
		}
	}
}
