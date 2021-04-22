using EasyHosting.Meta.Validators;
using EasyHosting.Models.Client.Serializers;
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
		public event EventHandler<Request> RequestSent;
		public event EventHandler<Request> RequestResponseReceived;
		public event EventHandler<StandardResponseWrapperSerializer> SignalReceived;

		protected Dictionary<long, Request> OnGoingRequests = new Dictionary<long, Request>();

		private JsonSerializer JsonSerializer = new JsonSerializer();

		private bool _Initialized = false;
		/// <summary>
		/// Określa, czy TcpClient został zainicjalizowany
		/// </summary>
		public bool Initialized { get { return _Initialized; } private set { _Initialized = value; } }

		private TcpClient _TcpClient = null;
		public TcpClient TcpClient { get { return _TcpClient; } private set { _TcpClient = value; } }

		private string _IpAddress;
		public string IpAddress { get { return _IpAddress; } private set { _IpAddress = value; } }

		private int _Port = 33564;
		public int Port { get { return _Port; } private set { _Port = value; } }

		private bool _Authorized = false;
		public bool Authorized { get { return _Authorized; } private set { _Authorized = value; } }

		protected long _CurrentRequestId = 1;
		public long CurrentRequestId { get { return _CurrentRequestId; } }

		private void Init() {
			TcpClient = new TcpClient(IpAddress, Port);
		}

		private BsonDataWriter _BsonWriter = null;
		protected BsonDataWriter BsonWriter {
			get {
				_BsonWriter = new BsonDataWriter(TcpClient.GetStream());
				return _BsonWriter;

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
				_BsonReader = new BsonDataReader(TcpClient.GetStream());
				return _BsonReader;

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

		public ClientSocket(string ipAddress, int port = 33564) {
			IpAddress = ipAddress;
			Port = port;

			Init();
		}

		/// <summary>
		/// Tworzy i wysyła zapytanie do serwera. Na odpowiedź na zapytanie należy nasłuchiwać na event'cie RequestResponseReceived
		/// </summary>
		/// <param name="requestData">Dane zapytania do wysłania, metoda obudowuje je dodatkowo odpowiednimi meta-danymi zapytania</param>
		public Request SendRequest(JObject requestData) {
			var requestId = CurrentRequestId;
			_CurrentRequestId++;

			var wrapper = new StandardRequestWrapperSerializer() {
				RequestCode = requestId,
				Data = requestData
			};
			var request = new Request(this, wrapper.GetApiObject(), requestId);

			OnGoingRequests.Add(requestId, request);
			request.Send();

			RequestSent?.Invoke(this, request);
			return request;
        }
		/// <summary>
		/// Wypisuje dane na strumień wyjściowy do serwera
		/// </summary>
		/// <param name="data"></param>
		public void WriteData(JObject data) {
			try {
				JsonSerializer.Serialize(BsonWriter, data);
            } catch(Exception e) {
				throw e; // Temporary; TODO: change
            }
		}
		/// <summary>
		/// Przetwarza dane otrzymane od serwera, określa czy są odpowiedzią na zapytanie, czy sygnałem od serwera
		/// </summary>
		/// <param name="data">Dane do przetworzenia</param>
		protected void ProcessReceivedData(JObject data) {
			var serializer = new StandardResponseWrapperSerializer(data);
			try {
				serializer.Validate();
            } catch(ValidationException e) {
				Console.WriteLine("Wystąpił błąd walidacji odpowiedzi: \n" + e.GetJson());
				return;
            }

			// Przypadek odpowiedzi na zapytanie wysłane przez socket
			if(serializer.CommunicateType == "REQUEST_RESPONSE" || serializer.CommunicateType == "REQUEST_ERROR" || serializer.CommunicateType == "AUTHORIZATION") {
				long id = serializer.RequestCode;

                if (OnGoingRequests.ContainsKey(id)) {
					Request request = OnGoingRequests[id];
					request.AttachResponse(serializer.Data);
					request.ResponseType = serializer.CommunicateType;

					OnGoingRequests.Remove(id);

					RequestResponseReceived?.Invoke(this, request);
                }
                else {
					Console.WriteLine("Zapytanie z identyfikatorem nie znalezione: " + id);
                }
            }
            else {
				SignalReceived?.Invoke(this, serializer);
            }
        }
		/// <summary>
		/// Sprawdza, czy serwer nadał komunikację do klienta, przetwarza komunikację od serwera, jeśli została nadana
		/// </summary>
		public void UpdateCommunication() {
			var stream = TcpClient.GetStream();
			JObject data;
            while (stream.DataAvailable) {
                try {
					data = JObject.Load(BsonReader);
                } catch(Exception e) {
					throw e; // temporary; TODO: change
                }
				ProcessReceivedData(data);
            }
        }
	}
}
