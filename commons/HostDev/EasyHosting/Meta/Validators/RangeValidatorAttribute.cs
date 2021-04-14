using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta.Validators
{
	/// <summary>
	/// Walidator uwzględniający zakres dozwolonych wartości. Typ danych dla walidacji musi być możliwy do porównań większe/mniejsze równe
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
	public class RangeValidatorAttribute : FieldValidatorAttribute
	{
		private readonly object _MinValue;
		/// <summary>
		/// Minimalna dozwolona wartość
		/// </summary>
		public object MinValue { get { return _MaxValue; } }

		private readonly object _MaxValue;
		/// <summary>
		/// Maksymalna dozwolona wartość
		/// </summary>
		public object MaxValue { get { return _MaxValue; } }

		private readonly bool _AllowNull;
		/// <summary>
		/// Czy dozwolony jest NULL
		/// </summary>
		public bool AllowNull { get { return _AllowNull; } }

		/// <param name="minValue">Minimalna dozwolona wartość (jeśli NULL, warunek nie jest sprawdzany)</param>
		/// <param name="maxValue">Maksymalna dozwolona wartość (jeśli NULL, warunek nie jest sprawdzany)</param>
		/// <param name="allowNull">Czy wartość może być NULLem</param>
		public RangeValidatorAttribute(object minValue = null, object maxValue = null, bool allowNull = false) {
			_MinValue = minValue;
			_MaxValue = maxValue;
			_AllowNull = allowNull;
		}

		/// <summary>
		/// Wykonuje walidację
		/// </summary>
		/// <param name="o">Obiekt do walidacji</param>
		/// <param name="throwException">Definiuje czy ma być wyrzucony wyjątek w przypadku błędu walidacji wszystkich alternatyw</param>
		/// <returns>Przepisuje wynik z pierwszej spełnionej alternatywy (zwalidowany obiekt, ew. konwertowany na określony typ danych))</returns>
		/// <exception cref="ValidationException">Wyjątek wyrzucany jeśli throwException==true oraz dane nie przeszły walidacji</exception>
		public override object Validate(object o, bool throwException = true) {
			if(o == null && AllowNull) {
				return null;
			}
			else if (o == null && !AllowNull) {
				AddError("NULL_NOT_ALLOWED", "Object cannot be NULL");
				if (throwException)
					ThrowException();
				return null;
            }

			if (!typeof(IComparable).IsInstanceOfType(o)) {
				AddError("OBJECT_NOT_VALID_FOR_COMPARISON", "Object is not valid for comparison");
				if (throwException)
					ThrowException();
				return null;
			}

			IComparable cmp = (IComparable)o;

			if(_MinValue != null) {
				if(cmp.CompareTo(_MinValue) < 0) {
					AddError("VALUE_TOO_SMALL", "Provided value was too small");
				}
			}
			if(_MaxValue != null) {
				if(cmp.CompareTo(_MaxValue) > 0) {
					AddError("VALUE_TO_BIG", "Provided value was too big");
				}
			}

			if(ErrorsCount == 0) {
				return o;
			}

			if (throwException)
				ThrowException();
			return null;
		}
	}
}
