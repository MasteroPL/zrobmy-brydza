using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta.Validators
{
	public class ValidationException : Exception
	{
		private Dictionary<string, List<ValidationError>> _Errors;
		public Dictionary<string, List<ValidationError>> Errors { get { return _Errors; } }

		private void Init(Dictionary<string, List<ValidationError>> errors, ValidationException originException) {
			if (errors == null) {
				_Errors = new Dictionary<string, List<ValidationError>>();
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

		public ValidationException(Dictionary<string, List<ValidationError>> errors = null, ValidationException originException = null) : base() {
			Init(errors, originException);
		}
		public ValidationException(List<ValidationError> errors) : base() {
			Init(new Dictionary<string, List<ValidationError>>() {
				{ "__GLOBAL__", errors }
			}, null);
		}

		/// <summary>
		/// Zwraca wszystkie przypisane błędy w jednej liście (bez podziału na pola, dla których te błędy zostały przypisane)
		/// </summary>
		/// <returns>Lista błędów</returns>
		public List<ValidationError> GetErrorsList() {
			var result = new List<ValidationError>();

			foreach(KeyValuePair<string, List<ValidationError>> errors in Errors) {
				result.AddRange(errors.Value);
			}

			return result;
		}
	}
}
