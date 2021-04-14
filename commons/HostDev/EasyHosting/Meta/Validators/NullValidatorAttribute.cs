using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta.Validators
{
	/// <summary>
	/// Defiuje, czy pole może być NULLem
	/// </summary>
	public class NullValidatorAttribute : FieldValidatorAttribute
	{
		/// <summary>
		/// Określa sposób walidacji (pozwala lub blokuje wartość NULL)
		/// </summary>
		private bool CanBeNull;

		public NullValidatorAttribute(bool canBeNull = false) {
			this.CanBeNull = canBeNull;
		}

		/// <summary>
		/// Wykonuje walidację
		/// </summary>
		/// <param name="o">Obiekt do walidacji</param>
		/// <param name="throwException">Definiuje czy ma być wyrzucony wyjątek w przypadku błędu walidacji wszystkich alternatyw</param>
		/// <returns>Przepisuje wynik z pierwszej spełnionej alternatywy (zwalidowany obiekt, ew. konwertowany na określony typ danych))</returns>
		/// <exception cref="ValidationException">Wyjątek wyrzucany jeśli throwException==true oraz dane nie przeszły walidacji</exception>
		public override object Validate(object o, bool throwException = true) {
			if(o == null) {
				if (!CanBeNull) {
					AddError("NULL_VALUE", "Field cannot be null");
					if (throwException) {
						ThrowException();
					}
				}
			}

			return o;
		}
	}
}
