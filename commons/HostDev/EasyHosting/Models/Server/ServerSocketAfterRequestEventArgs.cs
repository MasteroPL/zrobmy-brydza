using EasyHosting.Models.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace EasyHosting.Models.Server
{
	public class ServerSocketAfterRequestEventArgs
	{
		public TcpClient RequestingClient;
	}
}
