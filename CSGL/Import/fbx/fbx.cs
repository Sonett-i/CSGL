using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL
{
	/* Bytes: 0-20 
	 * 
	 * 
	 * 
	 */

	public class FBX
	{
		public static FBX Import(byte[] data)
		{
			int pointer = 0;
			char[] header = new char[27];

			while (pointer < data.Length)
			{
				byte currentByte = data[pointer++];
				char currentASCII = (char)currentByte;

				if (pointer <= 27)
				{
					header[pointer-1] = (char)currentByte;
				}


				if (currentByte == 0)
				{

				}
			}

			return null;
		}
	}
}
