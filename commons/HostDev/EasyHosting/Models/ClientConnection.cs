using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace EasyHosting.Models
{
	public class ClientConnection
	{
		private uint _BufferSize = 16384; // 16KB
		/// <summary>
		/// Zdefiniowany rozmiar bufora sczytującego z połączeń przychodzących
		/// </summary>
		public uint BufferSize { get { return _BufferSize; } private set { _BufferSize = value; } }

		private TcpClient _TcpClient = null;
		public TcpClient TcpClient { get { return _TcpClient; } private set { _TcpClient = TcpClient; } }

		public ConnectionState ConnectionState { get; protected set; }

		public bool DataAvailable {
			get {
				return _TcpClient.GetStream().DataAvailable;
			}
		}


		public ClientConnection(TcpClient tcpClient, uint bufferSize = 16384) {
			TcpClient = tcpClient;
			BufferSize = bufferSize;
		}


	}
}
