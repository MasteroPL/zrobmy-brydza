using EasyHosting.Meta;
using EasyHosting.Meta.Validators;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Server.Serializers
{
	public class ErrorSerializer : BaseSerializer
	{
		public static ErrorSerializer CreateInstance(string errorCode, string errorMessage) {
			return new ErrorSerializer() {
				ErrorCode = errorCode,
				ErrorMessage = errorMessage
			};
		}

		[SerializerField(apiName: "error_code")]
		[TextLengthRangeValidator(minLength: 1, maxLength: 20)]
		public string ErrorCode;

		[TextLengthRangeValidator(minLength: 0, maxLength: 100)]
		[SerializerField(apiName: "error_message")]
		public string ErrorMessage;

		public ErrorSerializer() : base() { }
		public ErrorSerializer(JObject data) : base(data) { }
	}
}
