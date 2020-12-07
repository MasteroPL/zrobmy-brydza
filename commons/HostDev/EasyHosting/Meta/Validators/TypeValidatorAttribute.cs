using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta.Validators
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
	public class TypeValidatorAttribute : FieldValidatorAttribute
	{
		private readonly Type _Type;
		public Type Type { get { return _Type; } }

		public TypeValidatorAttribute(Type type) {
			_Type = type;
		}

		public override object Validate(object o, bool throwException = true) {
			if(o == null) {
				return null;
			}

			if (_Type.IsInstanceOfType(o)) {
				return o;
			}

			AddError("INVALID_TYPE", "Failed to parse value into type " + Type);
			if (throwException)
				ThrowException();

			return null;
		}
	}
}
