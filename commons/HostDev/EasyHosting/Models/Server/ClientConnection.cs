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
	public class ClientConnection : IDisposable{
		public event EventHandler BeforeDispose;

		private bool _Disposed = false;
		public bool Disposed { get { return _Disposed; } }

		/// <summary>
		/// Komunikaty typu "PUSH", czyli wysałane z serwera do użytkownika. Nie są to odpowiedzi do zapytań
		/// </summary>
		private LinkedList<JObject> CommunicatesQueue = new LinkedList<JObject>();

		private ServerSocket _ServerSocket;
		/// <summary>
		/// ServerSocket z którym klient jest połączony
		/// </summary>
		public ServerSocket ServerSocket { get { return _ServerSocket; } private set { _ServerSocket = value; } }

		private Session _Session = new Session();
		/// <summary>
		/// Sesja przypisana do klienta
		/// </summary>
		public Session Session { get { return _Session; } protected set { _Session = value; } }

		private JsonSerializer JsonSerializer = new JsonSerializer();
		private BsonDataWriter _BsonWriter = null;
		/// <summary>
		/// Prefedefiniowany BsonWriter do serializacji binarnej komunikacji z klientem
		/// </summary>
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
		/// <summary>
		/// Predefiniowany BsonReader do deserializacji binarnej komunikacji z klientem
		/// </summary>
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

		private DateTime ConnectedAt = DateTime.Now;

		public TimeSpan GetConnectionTime (){
			return DateTime.Now - ConnectedAt;
		}

		private TcpClient _TcpClient = null;
		/// <summary>
		/// Fizyczne połączenie klienta
		/// </summary>
		public TcpClient TcpClient { get { return _TcpClient; } protected set { this._TcpClient = value; } }

		/// <summary>
		/// Określa czy klient nadał jakieś dane
		/// </summary>
		public bool DataAvailable {
			get {
				return _TcpClient.GetStream().DataAvailable;
			}
		}

		/// <summary>
		/// Pobiera dane od klienta
		/// </summary>
		/// <returns></returns>
		public JObject GetData() {
			try {
				var data = JToken.Load(BsonReader);
				return (JObject)data;
            } catch(Exception e) {
				throw e; // temporary; TODO: change
            }
        }

		/// <summary>
		/// Wpisuje dane do strumienia komunikacji z klientem
		/// </summary>
		/// <param name="data">Dane do wpisania</param>
		public void WriteData(JObject data) {
            try {
				JsonSerializer.Serialize(BsonWriter, data);
            } catch(Exception e) {
				throw e; // temporary; TODO: change
            }
        }

		/// <summary>
		/// Dodaje nowy komunikat do kolejki. Po przetworzeniu zapytań wszystkie komunikaty z kolejki są wysyłane do użytkownika
		/// </summary>
		/// <param name="communicate">Komunikat</param>
		public void AddCommunicate(JObject communicate) {
			CommunicatesQueue.AddLast(communicate);
        }
		/// <summary>
		/// Metoda wysyła wszystkie zakolejkowane komunikaty
		/// 
		/// Powinna być wywoływana tylko przez bazową klasę "ServerSocket"
		/// </summary>
		public void SendCommunicates() {
			foreach(var communicate in CommunicatesQueue) {
				WriteData(communicate);
            }
			CommunicatesQueue.Clear();
        }

		/// <summary>
		/// Fizyczna wysyłka danych po ich wpisaniu do strumienia
		/// </summary>
		public void Flush() {
			var stream = TcpClient.GetStream();
			stream.Flush();
        }

        public void Dispose() {
			if (!Disposed) {
				_Disposed = true;
				BeforeDispose?.Invoke(this, null);
				TcpClient.Close();
				TcpClient.Dispose();
			}
        }

        public ClientConnection(TcpClient tcpClient, ServerSocket serverSocket = null) {
			this.TcpClient = tcpClient;
			this.ServerSocket = serverSocket;
		}
	}
}
