using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Actions
{
	public class ActionsResponseSerializer : BaseSerializer
	{
		[SerializerField(apiName: "actions")]
		public ActionResponseSerializer[] Actions;

		public ActionsResponseSerializer() : base() { }
		public ActionsResponseSerializer(JObject data) : base(data) { }
	}
}
