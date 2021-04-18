using EasyHosting.Meta;
using EasyHosting.Models.Client;
using EasyHosting.Models.Client.Serializers;
using EasyHosting.Models.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSocketTesting
{
	class Program
	{
		static void OnRequestResponse(object sender, Request request) {
			Console.WriteLine(request.ResponseData);
        }
		static void OnSignal(object sender, StandardResponseWrapperSerializer signal) {
			Console.WriteLine(signal.Data);
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

			clientSocket.SendRequest(authData.GetApiObject());

			while (true){
				clientSocket.UpdateCommunication();
            }
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
