using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace EasyHosting.Models
{
	public class ClientSocket
	{
		private bool _Initialized = false;
		/// <summary>
		/// Określa, czy TcpClient został zainicjalizowany
		/// </summary>
		public bool Initialized { get { return _Initialized; } private set { _Initialized = value; } }

		private TcpClient _TcpClient = null;
		public TcpClient TcpClient { get { return _TcpClient; } private set { _TcpClient = value; } }

		private uint _BufferSize = 16384; // 16KB
		/// <summary>
		/// Zdefiniowany rozmiar bufora nadającego i odbierającego
		/// </summary>
		public uint BufferSize { get { return _BufferSize; } private set { _BufferSize = value; } }

		private string _IpAddress;
		public string IpAddress { get { return _IpAddress; } private set { _IpAddress = value; } }

		private int _Port = 33564;
		public int Port { get { return _Port; } private set { _Port = value; } }

		private void Init() {
			TcpClient = new TcpClient(IpAddress, Port);
		}

		public ClientSocket(string ipAddress, int port = 33564, uint bufferSize = 16384) {
			IpAddress = ipAddress;
			Port = port;
			BufferSize = bufferSize;

			Init();
		}

		public void Send(string message) {
			string messageToSend = message;
			int byteCount = Encoding.ASCII.GetByteCount(messageToSend + 1);
			byte[] sendData = Encoding.ASCII.GetBytes(messageToSend);

			NetworkStream stream = TcpClient.GetStream();
			stream.Write(sendData, 0, sendData.Length);
			Console.WriteLine("sending data to server...");

			StreamReader sr = new StreamReader(stream);
			string response = sr.ReadLine();

			Console.WriteLine(response);
			Console.ReadKey();
		}
	}
}
