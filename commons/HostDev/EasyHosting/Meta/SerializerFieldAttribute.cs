using EasyHosting.Meta.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class SerializerFieldAttribute : Attribute
	{
		private readonly string _ApiName;
		public string ApiName { get { return _ApiName; } }

		private readonly bool _Required;
		public bool Required { get { return _Required; } }

		private readonly object _Default = null;
		public object Default { get { return Default; } }

		private readonly FieldValidator[] _Validators = null;
		public FieldValidator[] Validators { get { return _Validators; } }

		public SerializerFieldAttribute(string apiName, bool required = true, object defaultValue = null) {
			if (ApiName == "__GLOBAL__") {
				throw new ConfigurationException("__GLOBAL__ is a reserved namespace");
			}

			_ApiName = apiName;
			_Required = required;

			if (!required) {
				if (defaultValue == null) {
					_Default = null;
				}
				else {
					_Default = defaultValue;
				}
			}
		}
	}
}
