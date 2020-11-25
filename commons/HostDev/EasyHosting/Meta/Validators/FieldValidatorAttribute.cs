using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta.Validators
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
	public abstract class FieldValidatorAttribute : Attribute
	{
		private readonly List<ValidationError> _Errors = new List<ValidationError>();
		public List<ValidationError> Errors { get { return _Errors; } } 

		public string ErrorsText { 
			get {
				StringBuilder result = new StringBuilder();
				foreach(var err in Errors) {
					result.Append(err.ErrorMessage);
					result.Append(";");
				}
				return result.ToString();
			} 
	}

		public int ErrorsCount { get { return _Errors.Count; } }
		
		/// <summary>
		/// Dodaje treść błędu do listy wszystkich błędów które wystąpiły podczas walidacji
		/// </summary>
		/// <param name="fieldName">Nazwa pola, dla którego wystąpił błąd</param>
		/// <param name="errorMessage"></param>
		protected void AddError(string errorCode, string errorMessage) {
			_Errors.Add(new ValidationError {
				ErrorCode = errorCode,
				ErrorMessage = errorMessage
			});
		}
		protected void AddErrors(IEnumerable<ValidationError> errors) {
			_Errors.AddRange(errors);
		}
		/// <summary>
		/// Rzuca wszystkie dodane błędy spakowane w pojedynczy wyjątek "ValidationException"
		/// </summary>
		protected void ThrowException() {
			throw new ValidationException(_Errors);
		}

		public abstract object Validate(object o, bool throwException = true);
	}
}
