using EasyHosting.Meta;
using EasyHosting.Meta.Validators;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EasyHosting.Models.Serialization
{
	/// <summary>
	/// Bazowa klasa dla serializatorów danych wysyłanych i odpowiedzi dla klientów
	/// </summary>
	public class BaseSerializer
	{
		/// <summary>
		/// Przechowuje oryginalny obiekt JSONa przekazany do serializatora
		/// </summary>
		public JObject DataOrigin { get; private set; }

		private Dictionary<FieldInfo, List<ValidationError>> _Errors = null;
		/// <summary>
		/// Słownik błędów, które wystąpiły podczas walidacji (nazwa pola -> lista błędów dla pola)
		/// </summary>
		public Dictionary<FieldInfo, List<ValidationError>> Errors { get { return _Errors; } private set { _Errors = value; } }

		private void AddError(FieldInfo field, ValidationError error) {
			if (!_Errors.ContainsKey(field)) {
				_Errors.Add(field, new List<ValidationError>());
			}
			_Errors[field].Add(error);
		}
		protected void AddError(string fieldName, ValidationError error) {
			if(fieldName == null) {
				AddError((FieldInfo) null, error);
			}
			
			var fieldInfo = this.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

			if(fieldInfo == null) {
				throw new ArgumentException("Invalid field name. Could not find specified field");
			}

			AddError(fieldInfo, error);
		}
		/// <summary>
		/// Dodaje błąd do listy błędów dla wybranego pola
		/// </summary>
		/// <param name="fieldName">Nazwa pola</param>
		/// <param name="errorCode">Kod błędu</param>
		/// <param name="errorMessage">Treść błędu</param>
		public void AddError(string fieldName, string errorCode, string errorMessage) {
			AddError(fieldName, new ValidationError { ErrorCode = errorCode, ErrorMessage = errorMessage });
		}
		private void AddErrors(FieldInfo field, IEnumerable<ValidationError> errors) {
			foreach(var error in errors) {
				AddError(field, error);
			}
		}
		protected void AddErrors(string fieldName, IEnumerable<ValidationError> errors) {
			if (fieldName == null) {
				AddErrors((FieldInfo)null, errors);
			}

			var fieldInfo = this.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

			AddErrors(fieldInfo, errors);
		}
		protected void AddErrors(Dictionary<string, List<ValidationError>> errors) {
			foreach (var keyValuePair in errors) {
				AddErrors(keyValuePair.Key, keyValuePair.Value);
			}
		}

		public void ThrowException() {
			throw new ValidationException(Errors);
		}

		private void Init() {
			var fields = this.GetType().GetFields().Where(
				prop => Attribute.IsDefined(prop, typeof(SerializerFieldAttribute), false)
			);

			foreach (var field in fields) {
				var fieldMetas = (SerializerFieldAttribute[])field.GetCustomAttributes(typeof(SerializerFieldAttribute), false);

				// Only a single definition allowed
				if (fieldMetas.Length > 1) {
					throw new ConfigurationException("Invalid configuration for SerializerField, field name: " + field.Name + "; in class: " + this.GetType().Name + "; Only accepts 1 definition of SerializerFieldAttribute");
				}
			}

			if (_Errors == null) {
				_Errors = new Dictionary<FieldInfo, List<ValidationError>>();
			}
			else {
				_Errors.Clear();
			}
		}

		/// <summary>
		/// Server -> User   Serializer constructor
		/// </summary>
		public BaseSerializer() {
			Init();
		}
		/// <summary>
		/// User -> Server   Serializer constructor
		/// </summary>
		/// <param name="data"></param>
		public BaseSerializer(JObject data) {
			Init();
			SetData(data);
		}

		/// <summary>
		/// Ustawia dane źródłowe dla serializatora
		/// </summary>
		/// <param name="data">Dane źródłowe dla serializatora</param>
		public virtual void SetData(JObject data) {
			DataOrigin = data;

			if (_Errors == null) {
				_Errors = new Dictionary<FieldInfo, List<ValidationError>>();
			}
			else {
				_Errors.Clear();
			}
		}

		public virtual void Validate(bool throwException = true) {
			bool fieldValid;
			JObject data = DataOrigin;
			object fieldValue = null;

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
						fieldValue = fieldMeta.Default;
					}
				}
				else {
					var currentData = data[fieldMeta.ApiName];
					// Przypadek zagniezdzonej tablicy serializatorow
					if (field.FieldType.IsArray && typeof(BaseSerializer).IsAssignableFrom(field.FieldType.GetElementType())) {
						try {
							BaseSerializer serializer;
							JArray arrayData = (JArray)currentData;

							Type serializerType = field.FieldType.GetElementType();
							BaseSerializer[] serializers = (BaseSerializer[])Array.CreateInstance(serializerType, arrayData.Count);
							int index = 0;
							foreach(var obj in arrayData) {
								serializer = (BaseSerializer)Activator.CreateInstance(serializerType);
								serializer.SetData((JObject)obj);
								serializer.Validate();
								serializers[index] = serializer;
								index++;
							}
							fieldValue = serializers;
						} catch(Exception e) {
							AddError(fieldMeta.ApiName, "INVALID_VALUE", "Could not convert provided value to type required by field.");
							fieldValid = false;
						}
					}
					// Przypadek zagniezdzonych serializatorów
					else if (typeof(BaseSerializer).IsAssignableFrom(field.FieldType)) {
						try {
							BaseSerializer serializer = (BaseSerializer)Activator.CreateInstance(field.FieldType);
							serializer.SetData((JObject)currentData);
							fieldValue = serializer;
						} catch(Exception e) {
							AddError(fieldMeta.ApiName, "INVALID_VALUE", "Could not convert provided value to type required by field.");
							fieldValid = false;
						}
					}
					// Przypadek zwykłej wartości podstawowej
					else {
						try {
							fieldValue = data[fieldMeta.ApiName].ToObject(field.FieldType);
						} catch (Exception e) {
							AddError(fieldMeta.ApiName, "INVALID_VALUE", "Could not convert provided value to type required by field.");
							fieldValid = false;
						}
					}
				}

				// Validators section
				if (fieldValid) {
					var fieldValidators = (FieldValidatorAttribute[])field.GetCustomAttributes(typeof(FieldValidatorAttribute), false);
					object validatedValue = fieldValue;
					foreach (var validator in fieldValidators) {
						validatedValue = validator.Validate(validatedValue, false); // We don't want an exception to be raised
						if (validator.ErrorsCount > 0) {
							AddErrors(field.Name, validator.Errors);
							break;
						}
					}
					if(Errors.Count == 0)
						field.SetValue(this, validatedValue);
				}
			}

			if(throwException && Errors.Count > 0) {
				ThrowException();
			}
		}
	
		/// <summary>
		/// Zwraca JObject o formacie zdefiniowanym jako format API (W atrybucie SerializerField argument apiName)
		/// </summary>
		/// <returns></returns>
		public virtual JObject GetApiObject() {
			JObject result = new JObject();

			var fields = this.GetType().GetFields().Where(
				prop => Attribute.IsDefined(prop, typeof(SerializerFieldAttribute), false)
			);

			foreach(var field in fields) {
				var fieldMetas = (SerializerFieldAttribute[])field.GetCustomAttributes(typeof(SerializerFieldAttribute), false);
				var fieldMeta = fieldMetas[0]; // In constructor it's already verified that there is only 1 attribute of type SerializerFieldAttribute assigned to 1 field

				if (field.FieldType.IsArray && typeof(BaseSerializer).IsAssignableFrom(field.FieldType.GetElementType())) {
					BaseSerializer[] sourceArr = (BaseSerializer[])field.GetValue(this);
					JObject[] targetArr = new JObject[sourceArr.Length];

					for(int i = 0; i < sourceArr.Length; i++) {
						if (sourceArr[i] != null)
							targetArr[i] = sourceArr[i].GetApiObject();
						else
							targetArr[i] = null;
					}
					result.Add(fieldMeta.ApiName, JToken.FromObject(targetArr));
				}
				else if (typeof(BaseSerializer).IsAssignableFrom(field.FieldType)) {
					var tmp = (BaseSerializer)field.GetValue(this);
					if (tmp != null) {
						result.Add(fieldMeta.ApiName, tmp.GetApiObject());
                    }
                    else {
						result.Add(fieldMeta.ApiName, null);
                    }
				}
				else {
					var val = field.GetValue(this);
					if (val != null)
						result.Add(fieldMeta.ApiName, JToken.FromObject(field.GetValue(this)));
					else
						result.Add(fieldMeta.ApiName, null);
				}
			}

			return result;
		}
	}
}
