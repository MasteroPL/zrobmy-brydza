using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Client
{
	public class ClientNetworkSocketException : Exception
	{
		public ClientNetworkSocketException() : base() { }
		public ClientNetworkSocketException(string message) : base(message) { }
		public ClientNetworkSocketException(string message, Exception innerException) : base(message, innerException) { }
		public ClientNetworkSocketException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
