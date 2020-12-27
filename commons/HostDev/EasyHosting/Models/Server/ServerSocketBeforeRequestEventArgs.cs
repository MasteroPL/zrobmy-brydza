using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace EasyHosting.Models.Server
{
	public class ServerSocketBeforeRequestEventArgs
	{
		public TcpClient RequestingClient;
	}
}
