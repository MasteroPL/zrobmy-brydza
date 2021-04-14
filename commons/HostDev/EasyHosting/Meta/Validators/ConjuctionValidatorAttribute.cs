using System;
using System.Collections.Generic;
using System.Text;
using EasyHosting.Models.Serialization;

namespace EasyHosting.Meta.Validators
{
	/// <summary>
	/// Klasa pozwalająca zdefiniować zestaw warunków, które pole musi spełniać (koniunkcja)
	/// </summary>
	public class ConjuctionValidatorAttribute : FieldValidatorAttribute
	{
		/// <summary>
		/// Zestaw walidatorów, które pola musi spełniać
		/// </summary>
		private FieldValidatorAttribute[] Validators;

		/// <summary>
		/// Klasa pozwalająca zdefiniować zestaw warunków, które pole musi spełniać
		/// </summary>
		/// <param name="validators">Ciąg kolejnych walidatorów</param>
		public ConjuctionValidatorAttribute(params FieldValidatorAttribute[] validators) {
			if (validators == null) {
				throw new ConfigurationException("Validators cannot be empty");
			}

			this.Validators = validators;
		}

		/// <summary>
		/// Wykonuje walidację w oparciu o zdefiniowane warunki. Jeśli żaden walidator nie zwraca błędów, zwraca wynik
		/// </summary>
		/// <param name="o">Obiekt do walidacji</param>
		/// <param name="throwException">Definiuje czy ma być wyrzucony wyjątek w przypadku błędu walidacji wszystkich alternatyw</param>
		/// <returns>Przepisuje wynik z pierwszej spełnionej alternatywy (zwalidowany obiekt, ew. konwertowany na określony typ danych))</returns>
		/// <exception cref="ValidationException">Wyjątek wyrzucany jeśli throwException==true oraz dane nie przeszły walidacji</exception>
		public override object Validate(object o, bool throwException = true) {
			object result = o;
			foreach(var validator in Validators) {
				result = validator.Validate(result, throwException = false);

				if(validator.ErrorsCount > 0) {
					AddErrors(validator.Errors);
				}

				if (throwException) {
					ThrowException();
				}
				else {
					return null;
				}
			}
			return result;
		}
	}
}
