using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Serialization
{
	public class InvalidInputDataException : SerializationException
	{
		public InvalidInputDataException() : base() { }
		public InvalidInputDataException(string message) : base(message) { }
		public InvalidInputDataException(string message, Exception innerException) : base(message, innerException) { }
		public InvalidInputDataException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
