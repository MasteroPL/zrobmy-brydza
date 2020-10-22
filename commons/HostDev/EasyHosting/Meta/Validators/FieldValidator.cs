using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta.Validators
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
	public abstract class FieldValidator : Attribute
	{
		private readonly List<ValidationError> _Errors = new List<ValidationError>();
		protected List<ValidationError> Errors { get { return _Errors; } } 

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
		/// <summary>
		/// Rzuca wszystkie dodane błędy spakowane w pojedynczy wyjątek "ValidationException"
		/// </summary>
		protected void RaiseException() {
			throw new ValidationException(_Errors);
		}

		public abstract object Validate(object o, bool raiseException = true);
	}
}
