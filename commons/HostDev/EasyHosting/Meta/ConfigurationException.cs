using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta
{
	/// <summary>
	/// Wyjątek konfiguracji, rzucany, jeśli konfiguracja została nieprawidłowo zdefiniowana. Wyrzucenie wyjątku indykuje błąd fizyczny (pod kątem poprawności konfiguracji) w kodzie.
	/// </summary>
	public class ConfigurationException : Exception
	{
		public ConfigurationException() : base() { }
		public ConfigurationException(string message) : base(message) { }
		public ConfigurationException(string message, Exception innerException) : base(message, innerException) { }
		public ConfigurationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
