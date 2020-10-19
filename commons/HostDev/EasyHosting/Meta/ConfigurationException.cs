using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta
{
	public class ConfigurationException : Exception
	{
		public ConfigurationException() : base() { }
		public ConfigurationException(string message) : base(message) { }
		public ConfigurationException(string message, Exception innerException) : base(message, innerException) { }
		public ConfigurationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
