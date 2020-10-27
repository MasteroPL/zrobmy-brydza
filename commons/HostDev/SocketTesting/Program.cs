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
			JObject obj = JObject.Parse("{ \"password\": \"123abc\" }");
			var serializer = new UserAuthorizationSerializer(obj);

			serializer.Validate();
			Console.WriteLine("OK");
		}

		static void Main(string[] args) {
			//Debug();
			//return;

			var serverSocket = new ServerSocket();
			serverSocket.Start();
		}
	}
}
