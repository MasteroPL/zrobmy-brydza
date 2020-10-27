using EasyHosting.Meta;
using EasyHosting.Meta.Validators;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyHosting.Models.Serialization
{
	public class BaseSerializer
	{
		public JObject DataOrigin { get; private set; }

		private Dictionary<string, List<ValidationError>> _Errors = null;
		public Dictionary<string, List<ValidationError>> Errors { get { return _Errors; } private set { _Errors = value; } }

		protected void AddError(string fieldName, ValidationError error) {
			if (!_Errors.ContainsKey(fieldName)) {
				_Errors.Add(fieldName, new List<ValidationError>());
			}
			_Errors[fieldName].Add(error);
		}
		protected void AddError(string fieldName, string errorCode, string errorMessage) {
			AddError(fieldName, new ValidationError { ErrorCode = errorCode, ErrorMessage = errorMessage });
		}

		protected void AddErrors(string fieldName, IEnumerable<ValidationError> errors) {
			if (!_Errors.ContainsKey(fieldName)) {
				_Errors.Add(fieldName, new List<ValidationError>());
			}
			_Errors[fieldName].AddRange(errors);
		}
		protected void AddErrors(Dictionary<string, List<ValidationError>> errors) {
			foreach (var keyValuePair in errors) {
				AddErrors(keyValuePair.Key, keyValuePair.Value);
			}
		}

		protected void ThrowException() {
			throw new ValidationException(Errors);
		}

		/// <summary>
		/// Server -> User   Serializer constructor
		/// </summary>
		public BaseSerializer() {

		}
		/// <summary>
		/// User -> Server   Serializer constructor
		/// </summary>
		/// <param name="data"></param>
		public BaseSerializer(JObject data) {
			DataOrigin = data;

			var fields = this.GetType().GetFields().Where(
				prop => Attribute.IsDefined(prop, typeof(SerializerFieldAttribute), false)
			);

			if(_Errors == null) {
				_Errors = new Dictionary<string, List<ValidationError>>();
			}
			else {
				_Errors.Clear();
			}

			foreach(var field in fields) {
				var fieldMetas = (SerializerFieldAttribute[])field.GetCustomAttributes(typeof(SerializerFieldAttribute), false);

				// Only a single definition allowed
				if(fieldMetas.Length > 1) {
					throw new ConfigurationException("Invalid configuration for SerializerField, field name: " + field.Name + "; in class: " + this.GetType().Name + "; Only accepts 1 definition of SerializerFieldAttribute");
				}
			}
		}

		protected virtual void Validate(bool throwException = true) {
			bool fieldValid;
			JObject data = DataOrigin;

			var fields = this.GetType().GetFields().Where(
				prop => Attribute.IsDefined(prop, typeof(SerializerFieldAttribute), false)
			);

			foreach (var field in fields) {
				var fieldMetas = (SerializerFieldAttribute[])field.GetCustomAttributes(typeof(SerializerFieldAttribute), false);

				fieldValid = true;

				// Basic cast section
				var fieldMeta = fieldMetas.First();
				if (!data.ContainsKey(fieldMeta.ApiName)) {
					if (fieldMeta.Required) {
						AddError(fieldMeta.ApiName, "MISSING_REQUIRED_FIELD", "This field is required.");
						fieldValid = false;
					}
					else {
						field.SetValue(this, fieldMeta.Default);
					}
				}
				else {
					try {
						field.SetValue(this, data[fieldMeta.ApiName].ToObject(field.FieldType));
					} catch (Exception e) {
						AddError(fieldMeta.ApiName, "INVALID_VALUE", "Could not convert provided value to type required by field.");
						fieldValid = false;
					}
				}

				// Validators section
				if (fieldValid) {
					var fieldValidators = (FieldValidatorAttribute[])field.GetCustomAttributes(typeof(FieldValidatorAttribute), false);
					object validatedValue = field.GetValue(this);
					foreach (var validator in fieldValidators) {
						validatedValue = validator.Validate(validatedValue, false); // We don't want an exception to be raised
						if (validator.ErrorsCount > 0) {
							AddErrors(field.Name, validator.Errors);
							break;
						}
						field.SetValue(this, validatedValue);
					}
				}
			}

			if(throwException && Errors.Count > 0) {
				ThrowException();
			}
		}
	}
}
