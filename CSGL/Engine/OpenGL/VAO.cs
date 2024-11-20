﻿using System;
using OpenTK.Graphics.OpenGL;

namespace CSGL.Engine.OpenGL
{
	internal class VAO : GLBuffer
	{

		public void LinkVBO(VBO VBO, uint layout)
		{
			VBO.Bind();


			VBO.Unbind();
		}



		public override void Dispose()
		{
			base.Dispose();
		}
	}
}
