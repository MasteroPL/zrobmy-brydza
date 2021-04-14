using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta.Validators
{
	/// <summary>
	/// Bazowa klasa definiowania atrybutów walidacji danych serializatora
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
	public abstract class FieldValidatorAttribute : Attribute
	{
		private readonly List<ValidationError> _Errors = new List<ValidationError>();
		/// <summary>
		/// Lista błędów walidacji
		/// </summary>
		public List<ValidationError> Errors { get { return _Errors; } } 

		/// <summary>
		/// Konwertuje listę błędów na tekst
		/// </summary>
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

		/// <summary>
		/// Liczba błędów
		/// </summary>
		public int ErrorsCount { get { return _Errors.Count; } }
		
		/// <summary>
		/// Dodaje treść błędu do listy wszystkich błędów które wystąpiły podczas walidacji
		/// </summary>
		/// <param name="errorCode">Kod błędu</param>
		/// <param name="errorMessage">Treść błędu</param>
		protected void AddError(string errorCode, string errorMessage) {
			_Errors.Add(new ValidationError {
				ErrorCode = errorCode,
				ErrorMessage = errorMessage
			});
		}
		/// <summary>
		/// Dodaje treść błędu do listy wszystkich błędów które wystąpiły podczas walidacji
		/// </summary>
		/// <param name="errors">Błędy do dodania</param>
		protected void AddErrors(IEnumerable<ValidationError> errors) {
			_Errors.AddRange(errors);
		}
		/// <summary>
		/// Rzuca wszystkie dodane błędy spakowane w pojedynczy wyjątek "ValidationException"
		/// </summary>
		protected void ThrowException() {
			throw new ValidationException(_Errors);
		}

		/// <summary>
		/// Wykonuje walidację danych
		/// </summary>
		/// <param name="o">Obiekt do zwalidowania</param>
		/// <param name="throwException">Określa czy wyrzucić wyjątek, jeśli walidacja się nie powiedzie</param>
		/// <returns>Zwalidowany obiekt</returns>
		/// <exception cref="ValidationException">Wyrzucany przy nieudanej walidacji</exception>
		public abstract object Validate(object o, bool throwException = true);
	}
}
