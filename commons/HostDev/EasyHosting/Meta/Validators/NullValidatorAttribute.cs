using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta.Validators
{
	public class NullValidatorAttribute : FieldValidatorAttribute
	{
		private bool CanBeNull;

		public NullValidatorAttribute(bool canBeNull = false) {
			this.CanBeNull = canBeNull;
		}

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
