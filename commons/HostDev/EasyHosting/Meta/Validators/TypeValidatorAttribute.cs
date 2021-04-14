using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta.Validators
{
	/// <summary>
	/// Walidator typu. Walidacja typu jest domyślnie obsługiwana przez serializatory poprzez typ pola, do którego przypisujemy wartość. Tego atrybutu można użyć jako dodatkową walidację, jeśli przyjmujemy tylko określone typy dziedzicące z bazowego
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
	public class TypeValidatorAttribute : FieldValidatorAttribute
	{
		private readonly Type _Type;
		/// <summary>
		/// Wymagany typ
		/// </summary>
		public Type Type { get { return _Type; } }

		private readonly bool _AllowInheritance;
		/// <summary>
		/// Określa, czy akceptowane są typy dziedzące z podanego
		/// </summary>
		public bool AllowInheritance { get { return _AllowInheritance; } }

		/// <param name="type">Wymagany typ</param>
		/// <param name="allowInheritance">Określa, czy akceptowane są typy dziedzące z podanego</param>
		public TypeValidatorAttribute(Type type, bool allowInheritance = true) {
			_Type = type;
			_AllowInheritance = allowInheritance;
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
				return null;
			}

			if (AllowInheritance && _Type.IsInstanceOfType(o)) {
				return o;
			}
			else if(!AllowInheritance && _Type.IsEquivalentTo(o.GetType())) {
				return o;
            }

			AddError("INVALID_TYPE", "Failed to parse value into type " + Type + " (AllowInheritance == " + AllowInheritance + ")");
			if (throwException)
				ThrowException();

			return null;
		}
	}
}
