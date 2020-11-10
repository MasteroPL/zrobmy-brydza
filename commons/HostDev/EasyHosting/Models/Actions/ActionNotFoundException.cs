using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Actions
{
	public class ActionNotFoundException : ActionManagerException
	{
		public string ActionName { get; private set; }

		public ActionNotFoundException(string actionName) : base() {
			ActionName = actionName;
		}
		public ActionNotFoundException(string actionName, string message) : base(message) {
			ActionName = actionName;
		}
		public ActionNotFoundException(string actionName, string message, Exception innerException) : base(message, innerException) {
			ActionName = actionName;
		}
		public ActionNotFoundException(string actionName, System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) {
			ActionName = actionName;
		}
	}
}
