using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta.Validators
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
	class TextLengthRangeValidatorAttribute : FieldValidatorAttribute
	{
		private readonly int _MinLength;
		public int MinValue { get { return _MaxLength; } }

		private readonly int _MaxLength;
		public int MaxValue { get { return _MaxLength; } }

		public TextLengthRangeValidatorAttribute(int minLength = -1, int maxLength = -1) {
			_MinLength = minLength;
			_MaxLength = maxLength;
		}
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
