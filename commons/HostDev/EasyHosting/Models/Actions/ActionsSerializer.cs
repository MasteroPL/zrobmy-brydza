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
		public const long NO_IDENTIFIER = -1;
		public const long BROADCAST_IDENTIFIER = -2;
		public const string RESERVED_NAMESPACE = "RESERVED_NAMESPACE";

		public static readonly long[] RESERVED_IDS = { 
			BROADCAST_IDENTIFIER
		};

		[SerializerField(apiName: "actions")]
		public ActionSerializer[] Actions;

		[SerializerField(apiName: "identifier", required: false, defaultValue: -1)]
		public long Identifier;

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

		public override void Validate(bool throwException = true) {
			base.Validate(throwException);

			if (Errors.Count > 0) {
				return;
			}

			// Nie pozwalamy na przysłanie predefiniowanych identyfikatorów
			if (Array.Exists(RESERVED_IDS, element => element == this.Identifier)) {
				AddError("Identifier", RESERVED_NAMESPACE, "Identifier predefined as reserved");
				if (throwException)
					ThrowException();
				return;
			}
		}
	}
}
