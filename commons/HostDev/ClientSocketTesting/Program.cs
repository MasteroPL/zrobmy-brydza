using EasyHosting.Meta;
using EasyHosting.Models.Actions;
using EasyHosting.Models.Client;
using EasyHosting.Models.Client.Serializers;
using EasyHosting.Models.Serialization;
using GameManagerLib.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetTableInfoSerializer = ServerSocket.Actions.GetTableInfo.ResponseSerializer;
using SitActionRequestSerializer = ServerSocket.Actions.Sit.RequestSerializer;
using SitPlayerOutActionRequestSerializer = ServerSocket.Actions.SitPlayerOut.RequestSerializer;
using System.Threading;

namespace ClientSocketTesting
{
	class Program
	{
		static bool Authorized = false;

		static ClientSocket ConnectToLobbyAndSit(string username, PlayerTag position)
		{
			var clientSocket = new ClientSocket("127.0.0.1");
			clientSocket.RequestResponseReceived += OnRequestResponse;
			clientSocket.SignalReceived += OnSignal;


			var authData = new AuthData()
			{
				LobbyId = "DEFAULT",
				Login = username,
				LobbyPassword = ""
			};

			var authRequest = clientSocket.SendRequest(authData.GetApiObject());

			while (authRequest.RequestState != RequestState.RESPONSE_RECEIVED)
			{
				clientSocket.UpdateCommunication();
			}

			var sitAction = new SitActionRequestSerializer()
			{
				PlaceTag = (int)position
			};
			var sitActionRequestData = WrapRequestData("sit", sitAction.GetApiObject());
			var sitActionRequest = clientSocket.SendRequest(sitActionRequestData.GetApiObject());

			while (sitActionRequest.RequestState != RequestState.RESPONSE_RECEIVED)
			{
				clientSocket.UpdateCommunication();
			}

			if(username.CompareTo("Macius") == 0) {
				
            }

			return clientSocket;
		}

		static void OnRequestResponse(object sender, Request request)
		{
			Console.WriteLine(request.ResponseData);
		}
		static void OnSignal(object sender, StandardResponseWrapperSerializer signal)
		{
			Console.WriteLine(signal.Data);
		}

		static ActionsSerializer WrapRequestData(string actionName, JObject data)
		{
			var result = new ActionsSerializer();
			result.Actions = new ActionSerializer[1];
			var tmp = new ActionSerializer();

			tmp.ActionName = actionName;
			tmp.ActionData = data;

			result.Actions[0] = tmp;

			return result;
		}

		static void Main(string[] args)
		{
			ClientSocket MaciusSocket = null, PawelekSocket = null, MarcinSocket = null;

			int counter = 0;

            while (/*true*/counter++ < 5)
            {
				if (MaciusSocket == null)MaciusSocket = ConnectToLobbyAndSit("Macius", PlayerTag.N);
                else MaciusSocket.UpdateCommunication();

				Thread.Sleep(1000);

				if (PawelekSocket == null) PawelekSocket = ConnectToLobbyAndSit("Pawełek", PlayerTag.S);
				else PawelekSocket.UpdateCommunication();

				Thread.Sleep(1000);

				if (MarcinSocket == null) MarcinSocket = ConnectToLobbyAndSit("Marcin", PlayerTag.W);
				else MarcinSocket.UpdateCommunication();

				Thread.Sleep(1000);
			}
            var sitPlayerOutAction = new SitPlayerOutActionRequestSerializer() {
                PlaceTag = (int)PlayerTag.W
            };
            var sitPlayerOutRequestData = WrapRequestData("sit-player-out", sitPlayerOutAction.GetApiObject());
            var sitPlayerOutRequest = MaciusSocket.SendRequest(sitPlayerOutRequestData.GetApiObject());

            while (sitPlayerOutRequest.RequestState != RequestState.RESPONSE_RECEIVED) {
                MaciusSocket.UpdateCommunication();
            }

            Console.WriteLine("OK");
		}
	}

	public class AuthData : BaseSerializer
	{
		[SerializerField(apiName: "lobby-id")]
		public string LobbyId;

		[SerializerField(apiName: "login")]
		public string Login;

		[SerializerField(apiName: "lobby-password")]
		public string LobbyPassword;
	}
}
