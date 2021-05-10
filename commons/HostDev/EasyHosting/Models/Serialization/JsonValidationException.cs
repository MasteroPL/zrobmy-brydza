using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Serialization {
    public class JsonValidationException : Exception {
		public JObject Errors;

		public JsonValidationException(JObject errors) : base() {
			Errors = errors;
		}
	}
}
