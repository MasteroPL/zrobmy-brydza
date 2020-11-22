using EasyHosting.Meta;
using EasyHosting.Meta.Validators;
using EasyHosting.Models.Serialization;
using EasyHosting.Models.Server.Serializers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyHosting.Models.Actions
{
	public class ActionResponseSerializer : BaseSerializer
	{
		/// <summary>
		/// Tworzy instancję tego serializatora wykonując walidację wprowadzonych danych
		/// </summary>
		/// <param name="status">Status odpowiedzi</param>
		/// <param name="actionName">Nazwa wykonanej akcji</param>
		/// <param name="data">Dane wynikowe wykonanej akcji</param>
		/// <param name="errors">Błędy wykonanej akcji</param>
		/// <returns></returns>
		public static ActionResponseSerializer CreateInstance(string status, string actionName, JObject data, IEnumerable<ErrorData> errors) {
			var result = new ActionResponseSerializer() {
				Status = status,
				ActionName = actionName,
				Data = data,
				ResponseErrors = new ErrorSerializer[errors.Count()]
			};

			int index = 0;
			foreach(var error in errors) {
				result.ResponseErrors[index] = ErrorSerializer.CreateInstance(error.ErrorCode, error.ErrorMessage);

				index++;
			}

			return result;
		}

		[SerializerField(apiName: "status")]
		[TextLengthRangeValidator(minLength: 1, maxLength: 20)]
		public string Status;

		[SerializerField(apiName: "name")]
		[TextLengthRangeValidator(minLength: 0, maxLength: 50)]
		public string ActionName;

		[SerializerField(apiName: "data")]
		public JObject Data;

		[SerializerField(apiName: "errors")]
		public ErrorSerializer[] ResponseErrors;

		public ActionResponseSerializer() : base() { }
		public ActionResponseSerializer(JObject data) : base(data) { }
	}

	public struct ErrorData
	{
		public string ErrorCode;
		public string ErrorMessage;
	}
}
