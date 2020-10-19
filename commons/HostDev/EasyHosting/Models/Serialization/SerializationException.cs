using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Serialization
{
	public class SerializationException : Exception
	{
		public SerializationException() : base() { }
		public SerializationException(string message) : base(message) { }
		public SerializationException(string message, Exception innerException) : base(message, innerException) { }
		public SerializationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
