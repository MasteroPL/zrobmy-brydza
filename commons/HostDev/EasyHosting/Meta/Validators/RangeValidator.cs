using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta.Validators
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
	public class RangeValidator : FieldValidator
	{
		private readonly object _MinValue;
		public object MinValue { get { return _MaxValue; } }

		private readonly object _MaxValue;
		public object MaxValue { get { return _MaxValue; } }

		private readonly bool _AllowNull;
		public bool AllowNull { get { return _AllowNull; } }

		public RangeValidator(object minValue = null, object maxValue = null, bool allowNull = false) {
			_MinValue = minValue;
			_MaxValue = maxValue;
			_AllowNull = allowNull;
		}

		public override object Validate(object o, bool raiseException = true) {
			if(o == null) {
				if (_AllowNull) {
					return o;
				}

				AddError("VALUE_IS_NULL", "Null is not allowed for this field");
				if (raiseException)
					RaiseException();
				return null;
			}

			if (!typeof(IComparable).IsInstanceOfType(o)) {
				AddError("OBJECT_NOT_VALID_FOR_COMPARISON", "Object is not valid for comparison");
				if (raiseException)
					RaiseException();
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

			if (raiseException)
				RaiseException();
			return null;
		}
	}
}
