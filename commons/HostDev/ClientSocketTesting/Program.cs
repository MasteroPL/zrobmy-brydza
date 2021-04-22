using EasyHosting.Meta;
using EasyHosting.Models.Actions;
using EasyHosting.Models.Client;
using EasyHosting.Models.Client.Serializers;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSocketTesting
{
	class Program
	{
		static bool Authorized = false;

		static void OnRequestResponse(object sender, Request request) {
			Console.WriteLine(request.ResponseData);
        }
		static void OnSignal(object sender, StandardResponseWrapperSerializer signal) {
			Console.WriteLine(signal.Data);
        }

		static ActionsSerializer WrapRequestData(string actionName, JObject data) {
			var result = new ActionsSerializer();
			result.Actions = new ActionSerializer[1];
			var tmp = new ActionSerializer();

			tmp.ActionName = actionName;
			tmp.ActionData = data;

			result.Actions[0] = tmp;

			return result;
        }

		static void Main(string[] args) {

			var clientSocket = new ClientSocket("127.0.0.1");
			clientSocket.RequestResponseReceived += OnRequestResponse;
			clientSocket.SignalReceived += OnSignal;


			var authData = new AuthData() {
				LobbyId = "DEFAULT",
				Login = "Macius",
				LobbyPassword = ""
			};

			var authRequest = clientSocket.SendRequest(authData.GetApiObject());

			while (authRequest.RequestState != RequestState.RESPONSE_RECEIVED){
				clientSocket.UpdateCommunication();
            }

			var tableInfoRequestData = WrapRequestData("get-table-info", null);
			var tableInfoRequest = clientSocket.SendRequest(tableInfoRequestData.GetApiObject());

			while(tableInfoRequest.RequestState != RequestState.RESPONSE_RECEIVED) {
				clientSocket.UpdateCommunication();
            }

			Console.WriteLine("OK");
		}
	}

	public class AuthData : BaseSerializer {
		[SerializerField(apiName:"lobby-id")]
		public string LobbyId;

		[SerializerField(apiName:"login")]
		public string Login;

		[SerializerField(apiName:"lobby-password")]
		public string LobbyPassword;
    }
}
