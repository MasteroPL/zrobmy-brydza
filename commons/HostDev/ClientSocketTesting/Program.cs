using EasyHosting.Models.Client;
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
			clientSocket.Send(data);
		}
	}
}
