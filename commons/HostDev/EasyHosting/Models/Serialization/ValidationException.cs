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
	}
}
