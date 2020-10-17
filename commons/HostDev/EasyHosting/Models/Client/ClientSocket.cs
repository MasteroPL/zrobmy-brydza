using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace EasyHosting.Models.Client
{
	public class ClientSocket
	{
		private JsonSerializer JsonSerializer = new JsonSerializer();

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

		private bool _Authorized = false;
		public bool Authorized { get { return _Authorized; } private set { _Authorized = value; } }

		public ConnectionState ConnectionState { get; protected set; }

		private void Init() {
			TcpClient = new TcpClient(IpAddress, Port);
		}

		private BsonDataWriter _BsonWriter = null;
		protected BsonDataWriter BsonWriter {
			get {
				if(_BsonWriter != null) {
					return _BsonWriter;
				}
				else {
					if (TcpClient != null) {
						_BsonWriter = new BsonDataWriter(TcpClient.GetStream());
						return _BsonWriter;
					}
					else {
						return null;
					}
				}
			}
		}
		private BsonDataReader _BsonReader = null;
		protected BsonDataReader BsonReader {
			get {
				if (_BsonReader != null) {
					return _BsonReader;
				}
				else {
					if (TcpClient != null) {
						_BsonReader = new BsonDataReader(TcpClient.GetStream());
						return _BsonReader;
					}
					else {
						return null;
					}
				}
			}
		}

		public ClientSocket(string ipAddress, int port = 33564, uint bufferSize = 16384) {
			IpAddress = ipAddress;
			Port = port;
			BufferSize = bufferSize;

			Init();

			ConnectionState = ConnectionState.IDLE;
		}

		public JObject Send(object data) {
			NetworkStream stream = TcpClient.GetStream();
			JsonSerializer.Serialize(BsonWriter, data);


			JObject response = null;
			try {
				response = JObject.Load(BsonReader);
			} catch (Exception e) {
				// TODO: handle bad request

				Console.WriteLine("Parsing response from the server failed");

				return null;
			}

			Console.WriteLine(response);
			Console.ReadKey();

			return response;
		}
	}
}
