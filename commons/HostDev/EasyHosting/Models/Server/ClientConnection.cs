using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using EasyHosting.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json;

namespace EasyHosting.Models.Server
{
	public class ClientConnection
	{
		private JsonSerializer JsonSerializer = new JsonSerializer();

		// 
		// Meta dane połączenia
		//

		private DateTime ConnectedAt = DateTime.Now;
		/// <summary>
		/// Długość istnienia połączenia
		/// </summary>
		public TimeSpan ConnectionTime {
			get {
				return DateTime.Now - ConnectedAt;
			}
		}

		private uint _BufferSize = 16384; // 16KB
		/// <summary>
		/// Zdefiniowany rozmiar bufora sczytującego z połączeń przychodzących
		/// </summary>
		public uint BufferSize { get { return _BufferSize; } protected set { _BufferSize = value; } }

		private TcpClient _TcpClient = null;
		public TcpClient TcpClient { get { return _TcpClient; } protected set { _TcpClient = value; } }

		private Func<ClientConnection, JObject, JObject> _RequestHandler;
		/// <summary>
		/// Funkcja do której wysyłane są informacje przechwycowanego zapytania
		/// </summary>
		public Func<ClientConnection, JObject, JObject> RequestHandler { get { return _RequestHandler; } protected set { _RequestHandler = value; } }

		private Func<ClientConnection, JObject, JObject> _AuthorizationHandler;
		/// <summary>
		/// Funkcja do której wysyłane są informacje autoryzacyjne (socket najpewniej wymaga autoryzacji przed udostępnieniem możliwości jakiejkolwiek komunikacji)
		/// </summary>
		public Func<ClientConnection, JObject, JObject> AuthorizationHandler { get { return _AuthorizationHandler; } protected set { _AuthorizationHandler = value; } }

		public ConnectionState ConnectionState { get; protected set; }

		public bool DataAvailable {
			get {
				return _TcpClient.GetStream().DataAvailable;
			}
		}

		private BsonDataWriter _BsonWriter = null;
		protected BsonDataWriter BsonWriter {
			get {
				if (_BsonWriter != null) {
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


		public ClientConnection(TcpClient tcpClient, Func<ClientConnection, JObject, JObject> authorizationHandler, Func<ClientConnection, JObject, JObject> requestHandler, uint bufferSize = 16384) {
			TcpClient = tcpClient;
			BufferSize = bufferSize;
			RequestHandler = requestHandler;
			AuthorizationHandler = authorizationHandler;
		}

		protected virtual void CheckForAny(Func<ClientConnection, JObject, JObject> handler) {
			var stream = TcpClient.GetStream();

			if (stream.DataAvailable) {
				//long incomingDataLength = stream.Length;

				//if (incomingDataLength > BufferSize) {
				//	// TODO: handle bad request - size too big exception
				//	stream.Flush();
				//	return;
				//}

				JObject requestData = null;

				try {
					requestData = JObject.Load(BsonReader);
				} catch (Exception e) {
					// TODO: handle bad request

					return;
				}


				JObject response = handler(this, requestData);

				// Serializacja odpowiedzi
				JsonSerializer.Serialize(BsonWriter, response);

				// Przesyłanie odpowiedzi
				stream.Flush();
			}
		}

		public virtual void CheckForRequest() {
			CheckForAny(this.RequestHandler);
		}

		public virtual void CheckForAuthorization() {
			CheckForAny(this.AuthorizationHandler);
		}
	}
}
