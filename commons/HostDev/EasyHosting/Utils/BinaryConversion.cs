using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Utils
{
	public static class BinaryConversion
	{
		public static string ReadBinary(byte[] bytes, int index, int count) {
			return Encoding.UTF8.GetString(bytes, index, count);
		}

		public static byte[] WriteBinary(string text) {
			return Encoding.UTF8.GetBytes(text);
		}
	}
}
