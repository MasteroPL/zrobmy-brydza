using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta.Validators
{
	/// <summary>
	/// Weryfikuje dozwoloną długość tekstu
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
	class TextLengthRangeValidatorAttribute : FieldValidatorAttribute
	{
		private readonly int _MinLength;
		/// <summary>
		/// Minimalna dozwolona długość tekstu (jeśli -1, nie jest sprawdzana)
		/// </summary>
		public int MinValue { get { return _MaxLength; } }

		private readonly int _MaxLength;
		/// <summary>
		/// Maksymalna dozowlona długość tekstu (jeśli -1, nie jest sprawdzana)
		/// </summary>
		public int MaxValue { get { return _MaxLength; } }

		/// <param name="minLength">Minimalna dozwolona długość tekstu (jeśli -1, nie jest sprawdzana)</param>
		/// <param name="maxLength">Maksymalna dozowlona długość tekstu (jeśli -1, nie jest sprawdzana)</param>
		public TextLengthRangeValidatorAttribute(int minLength = -1, int maxLength = -1) {
			_MinLength = minLength;
			_MaxLength = maxLength;
		}

		/// <summary>
		/// Wykonuje walidację
		/// </summary>
		/// <param name="o">Obiekt do walidacji</param>
		/// <param name="throwException">Definiuje czy ma być wyrzucony wyjątek w przypadku błędu walidacji wszystkich alternatyw</param>
		/// <returns>Przepisuje wynik z pierwszej spełnionej alternatywy (zwalidowany obiekt, ew. konwertowany na określony typ danych))</returns>
		/// <exception cref="ValidationException">Wyjątek wyrzucany jeśli throwException==true oraz dane nie przeszły walidacji</exception>
		public override object Validate(object o, bool throwException = true) {
			if (o == null) {
				return null;
			}

			if (!typeof(string).IsInstanceOfType(o)) {
				AddError("NOT_A_STRING", "Object is not of 'string' type");
				if (throwException)
					ThrowException();
				return null;
			}

			string cmp = (string)o;

			if (_MinLength != -1) {
				if (cmp.Length < _MinLength) {
					AddError("TEXT_TOO_SHORT", "Provided text was too short");
				}
			}
			if (_MaxLength != -1) {
				if (cmp.Length > _MaxLength) {
					AddError("TEXT_TOO_LONG", "Provided text was too long");
				}
			}

			if (ErrorsCount == 0) {
				return o;
			}

			if (throwException)
				ThrowException();
			return null;
		}
	}
}
