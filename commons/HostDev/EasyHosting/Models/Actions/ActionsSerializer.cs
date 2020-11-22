using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Actions
{
	public class ActionsSerializer : BaseSerializer
	{
		[SerializerField(apiName: "actions")]
		public ActionSerializer[] Actions;

		public ActionsSerializer() : base() { }
		public ActionsSerializer(JObject data) : base(data) { }

		public ActionMeta[] GetActionsMeta() {
			var actionsMeta = new ActionMeta[Actions.Length];

			int index = 0;
			foreach(var action in Actions) {
				actionsMeta[index] = action.GetActionMeta();

				index++;
			}

			return actionsMeta;
		}
	}
}
