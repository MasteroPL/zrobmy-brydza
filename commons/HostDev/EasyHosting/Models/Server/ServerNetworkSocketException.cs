using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Server
{
	public class ServerNetworkSocketException : Exception
	{
		public ServerNetworkSocketException() : base() { }
		public ServerNetworkSocketException(string message) : base(message) { }
		public ServerNetworkSocketException(string message, Exception innerException) : base(message, innerException) { }
		public ServerNetworkSocketException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

	}
}
