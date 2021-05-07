using EasyHosting.Meta.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta
{
	/// <summary>
	/// Określa pole do uwzględnienia w serializacji
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class SerializerFieldAttribute : Attribute
	{
		private readonly string _ApiName;
		/// <summary>
		/// Nazwa pola w komunikacji sieciowej
		/// </summary>
		public string ApiName { get { return _ApiName; } }

		private readonly bool _Required;
		/// <summary>
		/// Określa, czy pole jest wymagane
		/// </summary>
		public bool Required { get { return _Required; } }

		private readonly object _Default = null;
		/// <summary>
		/// Określa wartość domyślną dla pola (jeśli pole nie jest wymagane, powinno definiować wartość domyślną)
		/// </summary>
		public object Default { get { return _Default; } }

		private readonly FieldValidatorAttribute[] _Validators = null;
		/// <summary>
		/// Określa zestaw walidatorów dla pola
		/// </summary>
		public FieldValidatorAttribute[] Validators { get { return _Validators; } }

		/// <param name="apiName">Nazwa pola w komunikacji sieciowej</param>
		/// <param name="required">Określa, czy pole jest wymagane</param>
		/// <param name="defaultValue">Określa wartość domyślną dla pola (jeśli pole nie jest wymagane, powinno definiować wartość domyślną)</param>
		/// <exception cref="ConfigurationException">Wyjątek wyrzucany dla apiName == "__GLOBAL__". Jest to nazwa zarezerwowana.</exception>
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
