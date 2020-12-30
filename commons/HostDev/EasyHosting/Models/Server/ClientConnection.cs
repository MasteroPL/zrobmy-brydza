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
		private Session _Session = new Session();
		public Session Session { get { return _Session; } protected set { _Session = value; } }

		private JsonSerializer JsonSerializer = new JsonSerializer();
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

		private DateTime ConnectedAt = DateTime.Now;

		public TimeSpan GetConnectionTime (){
			return DateTime.Now - ConnectedAt;
		}

		private TcpClient _TcpClient = null;
		public TcpClient TcpClient { get { return _TcpClient; } protected set { this._TcpClient = value; } }

		public ConnectionState ConnectionState { get; protected set; }

		public bool DataAvailable {
			get {
				return _TcpClient.GetStream().DataAvailable;
			}
		}

		public JObject GetData() {
			try {
				var data = JObject.Load(BsonReader);
				return data;
            } catch(Exception e) {
				throw e; // temporary; TODO: change
            }
        }

		public void WriteData(JObject data) {
            try {
				JsonSerializer.Serialize(BsonWriter, data);
            } catch(Exception e) {
				throw e; // temporary; TODO: change
            }
        }

		public void Flush() {
			var stream = TcpClient.GetStream();
			stream.Flush();
        }

		public ClientConnection(TcpClient tcpClient) {
			this.TcpClient = tcpClient;
		}
	}
}
