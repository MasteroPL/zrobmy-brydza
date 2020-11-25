using EasyHosting.Models.Actions;
using EasyHosting.Models.Server;
using EasyHosting.Models.Server.Serializers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTesting
{
	class Program
	{
		public static void Debug() {
			JObject obj = JObject.Parse("{ \"actions\": [ { \"name\": \"test\", \"data\": null } ] }");
			var serializer = new ActionsSerializer(obj);

			serializer.Validate();
			var response = serializer.GetApiObject();
			Console.WriteLine("OK");
		}

		static void Main(string[] args) {
			Debug();
			return;

			var serverSocket = new ServerSocket();
			serverSocket.Start();
		}
	}
}
