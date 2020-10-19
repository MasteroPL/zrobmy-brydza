using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class SerializerFieldAttribute : Attribute
	{
		private string _ApiName;
		public string ApiName { get { return _ApiName; } }

		private string _LocalName;
		public string LocalName { get { return _LocalName; } }

		private Type _Type;
		public Type Type { get { return _Type; } }

		private bool _Required;
		public bool Required { get { return _Required; } }

		private object _Default = null;
		public object Default { get { return Default; } }

		private object GetDefaultValue(Type t) {
			if (t.IsValueType) {
				return Activator.CreateInstance(t);
			}
			return null;
		}

		public SerializerFieldAttribute(string apiName, string localName, Type type, bool required = true, object defaultValue = null) {
			if (ApiName == "__GLOBAL__" || LocalName == "__GLOBAL__") {
				throw new ConfigurationException("__GLOBAL__ is a reserved namespace");
			}

			_ApiName = apiName;
			_LocalName = localName;
			_Type = type;
			_Required = required;

			if (!required) {
				if(defaultValue == null) {
					_Default = GetDefaultValue(Type);
				}
				else {
					if (Type.IsInstanceOfType(defaultValue)) {
						_Default = defaultValue;
					}
					else {
						throw new ConfigurationException("Cannot assign a default value of different type than the one provided as argument");
					}
				}
			}
		}
	}
}
