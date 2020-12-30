using EasyHosting.Meta;
using EasyHosting.Models.Client;
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
		static void Main(string[] args) {
			var data = new SampleStruct {
				Field1 = 100,
				Field2 = "TEST"
			};

			var clientSocket = new ClientSocket("127.0.0.1");

			var auth = new AuthData();
			auth.Login = "Maciuś";
			auth.LobbyId = "DEFAULT";
			auth.LobbyPassword = "";
			clientSocket.Send(auth.GetApiObject());

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
