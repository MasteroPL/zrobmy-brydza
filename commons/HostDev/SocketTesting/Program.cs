using EasyHosting.Models.Actions;
using EasyHosting.Models.Server;
using EasyHosting.Models.Server.Config;
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

		static void Main(string[] args) {
			MainConfig.InitiateServerSocket();
			MainConfig.SERVER_SOCKET.Start();
		}
	}
}
