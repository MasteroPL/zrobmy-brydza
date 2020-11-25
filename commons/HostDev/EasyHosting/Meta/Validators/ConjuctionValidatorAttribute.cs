using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta.Validators
{
	/// <summary>
	/// Klasa pozwalająca zdefiniować zestaw warunków, które pole musi spełniać
	/// </summary>
	public class ConjuctionValidatorAttribute : FieldValidatorAttribute
	{
		private FieldValidatorAttribute[] Validators;

		/// <summary>
		/// Klasa pozwalająca zdefiniować zestaw warunków, które pole musi spełniać
		/// </summary>
		/// <param name="validators">Ciąg kolejnych, alternatywnych walidatorów</param>
		public ConjuctionValidatorAttribute(params FieldValidatorAttribute[] validators) {
			if (validators == null) {
				throw new ConfigurationException("Validators cannot be empty");
			}

			this.Validators = validators;
		}

		public override object Validate(object o, bool throwException = true) {
			object result = o;
			foreach(var validator in Validators) {
				result = validator.Validate(result, throwException = false);

				if(validator.ErrorsCount > 0) {
					AddErrors(validator.Errors);
				}

				if (throwException) {
					ThrowException();
				}
				else {
					return null;
				}
			}
			return result;
		}
	}
}
