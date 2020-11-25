﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Meta.Validators
{
	/// <summary>
	/// Klasa pozwalająca zdefiniować zestaw alternatyw pod kątem konfiguracyjnej walidacji pól
	/// </summary>
	public class AlternativeValidatorAttribute : FieldValidatorAttribute
	{
		private FieldValidatorAttribute[] AlternateValidators;
		private string ErrorCodeOnFail;

		/// <summary>
		/// Klasa pozwalająca zdefiniować zestaw alternatyw pod kątem konfiguracyjnej walidacji pól
		/// </summary>
		/// <param name="errorCodeOnFail">Kod błędu, który ma się zwrócić w przypadku nieudanej walidacji. Jeśli null, zostanie przypisany ALTERNATIVE_CHECK_FAILED</param>
		/// <param name="alternateValidators">Ciąg kolejnych, alternatywnych walidatorów</param>
		public AlternativeValidatorAttribute(string errorCodeOnFail, params FieldValidatorAttribute[] alternateValidators) {
			if(alternateValidators == null) {
				throw new ConfigurationException("Alternate validators cannot be empty");
			}

			this.AlternateValidators = alternateValidators;
			this.ErrorCodeOnFail = (errorCodeOnFail == null) ? "ALTERNATIVE_CHECK_FAILED" : errorCodeOnFail;
		}

		public override object Validate(object o, bool throwException = true) {
			StringBuilder altErrs = new StringBuilder();
			int currentIndex = 0;
			object result;

			foreach(var validator in AlternateValidators) {
				result = validator.Validate(o, false);
				if(validator.ErrorsCount > 0) {
					altErrs.Append("[");
					altErrs.Append(currentIndex);
					altErrs.Append("] ");
					altErrs.Append(validator.ErrorsText);
					altErrs.Append(";");
				}
				else {
					return result;
				}
			}

			AddError(this.ErrorCodeOnFail, altErrs.ToString());
			if (throwException) {
				ThrowException();
			}
			return null;
		}
	}
}