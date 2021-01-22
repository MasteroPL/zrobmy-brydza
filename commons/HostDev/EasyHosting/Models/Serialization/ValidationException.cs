using EasyHosting.Models.Server.Serializers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EasyHosting.Meta.Validators
{
	public class ValidationException : Exception
	{
		private Dictionary<FieldInfo, List<ValidationError>> _Errors;
		public Dictionary<FieldInfo, List<ValidationError>> Errors { get { return _Errors; } }

		private void Init(Dictionary<FieldInfo, List<ValidationError>> errors, ValidationException originException) {
			if (errors == null) {
				_Errors = new Dictionary<FieldInfo, List<ValidationError>>();
			}
			else {
				_Errors = errors;
			}

			if (originException != null) {
				foreach(var keyValuePair in originException._Errors) {
					if (!_Errors.ContainsKey(keyValuePair.Key)) {
						_Errors.Add(keyValuePair.Key, keyValuePair.Value);
					}
					else {
						_Errors[keyValuePair.Key].AddRange(keyValuePair.Value);
					}
				}
			}
		}

		public ValidationException(Dictionary<FieldInfo, List<ValidationError>> errors = null, ValidationException originException = null) : base() {
			Init(errors, originException);
		}
		public ValidationException(List<ValidationError> errors) : base() {
			Init(new Dictionary<FieldInfo, List<ValidationError>>() {
				{ null, errors }
			}, null);
		}

		/// <summary>
		/// Zwraca wszystkie przypisane błędy w jednej liście (bez podziału na pola, dla których te błędy zostały przypisane)
		/// </summary>
		/// <returns>Lista błędów</returns>
		public List<ValidationError> GetErrorsList() {
			var result = new List<ValidationError>();

			foreach(KeyValuePair<FieldInfo, List<ValidationError>> errors in Errors) {
				result.AddRange(errors.Value);
			}

			return result;
		}

		public JObject GetJson() {
			var resp = new ErrorResponseSerializer() {
				Status = "ERR",
				FieldErrors = null
			};

			var fieldErrors = new Dictionary<string, List<ValidationError>>();

			FieldInfo field;
			SerializerFieldAttribute[] fieldMetas;
			string fieldName;
			foreach(var keyValuePair in Errors) {
				field = keyValuePair.Key;

				if (field != null) {
					fieldMetas = (SerializerFieldAttribute[])field.GetCustomAttributes(typeof(SerializerFieldAttribute), false);

					try {
						// Pole które znalazło się na liście musi mieć SerializerFieldAttribute, jeśli nie ma, programista który kodował walidację danych jest idiotą
						fieldName = fieldMetas[0].ApiName;
					} catch(Exception e) {
						throw new Exception("Jeżeli ten wyjątek został wyrzucony, programista, który kodował walidację danych w serializatorze, który wyrzucił wyjątek walidacji jest idiotą i dodał błąd dla pola, które nie jest częścią interfejsu API (nie ma atrybutu SerializerField)", e);
                    }
				}
                else {
					fieldName = "__GLOBAL__";
                }

				fieldErrors.Add(fieldName, keyValuePair.Value);
            }

			resp.FieldErrors = fieldErrors;

			return resp.GetApiObject();
        }
	}
}
