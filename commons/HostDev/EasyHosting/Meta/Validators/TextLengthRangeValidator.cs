using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta.Validators
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
	class TextLengthRangeValidator : FieldValidator
	{
		private readonly int _MinLength;
		public int MinValue { get { return _MaxLength; } }

		private readonly int _MaxLength;
		public int MaxValue { get { return _MaxLength; } }

		private readonly bool _AllowNull;
		public bool AllowNull { get { return _AllowNull; } }

		public TextLengthRangeValidator(int minLength = -1, int maxLength = -1, bool allowNull = false) {
			_MinLength = minLength;
			_MaxLength = maxLength;
			_AllowNull = allowNull;
		}
		public override object Validate(object o, bool raiseException = true) {
			if (o == null) {
				if (_AllowNull) {
					return o;
				}

				AddError("VALUE_IS_NULL", "Null is not allowed for this field");
				if (raiseException)
					RaiseException();
				return null;
			}

			if (!typeof(string).IsInstanceOfType(o)) {
				AddError("NOT_A_STRING", "Object is not of 'string' type");
				if (raiseException)
					RaiseException();
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

			if (raiseException)
				RaiseException();
			return null;
		}
	}
}
