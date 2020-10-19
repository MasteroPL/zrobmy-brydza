using EasyHosting.Meta;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using EasyHosting.Models.Serialization;

namespace EasyHosting.Models.Server.Serializers
{
	public class UserAuthorizationSerializer : BaseSerializer
	{
		[SerializerField(apiName: "password", localName: "Password", type: typeof(string))]
		public string Password;

		public UserAuthorizationSerializer(JObject data) : base(data) { }
	}
}
