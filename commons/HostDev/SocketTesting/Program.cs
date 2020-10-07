using EasyHosting.Models;
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
			var serverSocket = new ServerSocket();
			serverSocket.Start();
		}
	}
}
