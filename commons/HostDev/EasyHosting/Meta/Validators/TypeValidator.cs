using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta.Validators
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
	public class TypeValidator : FieldValidator
	{
		private readonly Type _Type;
		public Type Type { get { return _Type; } }

		public TypeValidator(Type type) {
			_Type = type;
		}

		public override object Validate(object o, bool raiseException = true) {
			if (_Type.IsInstanceOfType(o)) {
				return o;
			}

			AddError("INVALID_TYPE", "Failed to parse value into type " + typeof(T));
			if (raiseException)
				RaiseException();

			return null;
		}
	}
}
