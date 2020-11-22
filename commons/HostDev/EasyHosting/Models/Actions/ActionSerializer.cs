using EasyHosting.Meta;
using EasyHosting.Meta.Validators;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Actions
{
	public class ActionSerializer : BaseSerializer
	{

		[SerializerField(apiName: "name")]
		[NullValidator(canBeNull: false)]
		[TextLengthRangeValidator(minLength: 0, maxLength: 50)]
		public string ActionName;

		[SerializerField(apiName: "data")]
		public JObject ActionData;

		public ActionSerializer() : base() { }
		public ActionSerializer(JObject data) : base(data) { }

		public ActionMeta GetActionMeta() {
			return new ActionMeta {
				Name = ActionName,
				Data = ActionData
			};
		}
	}
}
