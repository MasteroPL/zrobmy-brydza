using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta.Validators
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
	public class RangeValidatorAttribute : FieldValidatorAttribute
	{
		private readonly object _MinValue;
		public object MinValue { get { return _MaxValue; } }

		private readonly object _MaxValue;
		public object MaxValue { get { return _MaxValue; } }

		private readonly bool _AllowNull;
		public bool AllowNull { get { return _AllowNull; } }

		public RangeValidatorAttribute(object minValue = null, object maxValue = null) {
			_MinValue = minValue;
			_MaxValue = maxValue;
		}

		public override object Validate(object o, bool throwException = true) {
			if(o == null) {
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
