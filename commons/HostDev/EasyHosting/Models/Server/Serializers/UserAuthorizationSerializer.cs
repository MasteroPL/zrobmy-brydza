using EasyHosting.Meta;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using EasyHosting.Models.Serialization;
using EasyHosting.Meta.Validators;

namespace EasyHosting.Models.Server.Serializers
{
	public class UserAuthorizationSerializer : BaseSerializer
	{
		[SerializerField(apiName: "password")]
		[TextLengthRangeValidator(minLength: 0, maxLength: 20)]
		public string Password;

		public UserAuthorizationSerializer() : base() { }
		public UserAuthorizationSerializer(JObject data) : base(data) { }
	}
}
