using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Actions
{
	public class ActionManagerException : Exception
	{
		public ActionManagerException() : base() { }
		public ActionManagerException(string message) : base(message) { }
		public ActionManagerException(string message, Exception innerException) : base(message, innerException) { }
		public ActionManagerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
