using System;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Net;

namespace EasyHosting.Models.Server
{
    public abstract class ServerSocket {
        private List<ClientConnection> UnauthorizedConnections = new List<ClientConnection>();
        private List<ClientConnection> AuthorizedConnections = new List<ClientConnection>();

        protected TimeSpan TimeForAuthorization;

        private bool _Initialized = false;
        /// <summary>
        /// Określa, czy TcpListener został zainicjalizowany i nasłuchuje połączeń
        /// </summary>
        public bool Initialized { get { return _Initialized; } private set { _Initialized = value; } }

        private TcpListener _TcpListener = null;
        /// <summary>
        /// Zainicjalizowany TcpListener
        /// </summary>
        public TcpListener TcpListener { get { return _TcpListener; } private set { _TcpListener = value; } }


        private System.Net.IPAddress _IpAddress = System.Net.IPAddress.Any;
        public System.Net.IPAddress IpAddress { get { return _IpAddress; } private set { _IpAddress = value; } }

        private int _Port = 33564;
        public int Port { get { return _Port; } private set { _Port = value; } }

        protected readonly JObject AuthorizationSuccessfulResponse = JObject.Parse("{ \"authroization_status\": \"OK\" }");

        private void HandleIncommingConnections() {
            int acceptedCount = 0;
            while (TcpListener.Pending()) {
                TcpClient newClient = TcpListener.AcceptTcpClient();

                UnauthorizedConnections.Add(
                    new ClientConnection(
                        newClient
                    )
                );
                Console.WriteLine("Accepted connection from: " + ((IPEndPoint)newClient.Client.RemoteEndPoint).Address.ToString());
                acceptedCount++;
            }
        }

		private void Listen() {
            TcpListener = new TcpListener(IpAddress, Port);
            TcpListener.Start();

            List<ClientConnection> toRemove = new List<ClientConnection>();

            while (true) {
                // Komunikaty
                foreach(var connection in AuthorizedConnections) {
                    connection.SendCommunicates();
                }
                foreach(var connection in UnauthorizedConnections) {
                    connection.SendCommunicates();
                }

                // Zautoryzowane połączenia
                foreach (var connection in AuthorizedConnections) {
                    if (connection.DataAvailable) {
                        JObject data = connection.GetData();
                        JObject response = HandleRequest(connection, data);
                        connection.WriteData(response);
                        connection.Flush();

                        // TODO automatyczna obsługa wyjątków pod kątem zwracania nieprawidłowych odpowiedzi
                    }
                }

                // Niezautoryzowane połączenia
                toRemove.Clear();
                foreach (var connection in UnauthorizedConnections){
                    if (TimeSpan.Compare(connection.GetConnectionTime(), TimeForAuthorization) <= 0) {
                        if (connection.DataAvailable) {
                            JObject data = connection.GetData();
                            if (AuthorizeConnection(connection, data)) {
                                AuthorizedConnections.Add(connection);
                                toRemove.Add(connection);

                                // Informacja dla odbiorcy o poprawnej autoryzacji
                                connection.WriteData(AuthorizationSuccessfulResponse);
                                connection.Flush();
                            }
                        }
                    } 
                    else {
                        connection.TcpClient.Close();
                        toRemove.Add(connection);
                    }
                }

                // Finalizacja przenoszenie połączenia po zautoryzowaniu lub zamknięciu
                foreach (var connection in toRemove) {
                    UnauthorizedConnections.Remove(connection);
                }

                // Nowe połączenia
                HandleIncommingConnections();
            }
        }

        public ServerSocket(System.Net.IPAddress ipAddress = null, int port = 33564, int secondsForAuthorization = 10) {
            if (ipAddress != null) {
                IpAddress = ipAddress;
            }
            Port = port;
            TimeForAuthorization = TimeSpan.FromSeconds(secondsForAuthorization);
        }

        public void Start() {
            Listen();
        }

        public void StartInThread() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Metoda wywoływana po uzyskaniu pierwszego strumienia danych z 
        /// niezautoryzowanego połączenia. Powinna zwalidować poprawność 
        /// danych autoryzacyjnych w przychodzącym strumieniu danych
        /// i zwrócić "true" jeśli autoryzacja przebiegła pomyslnie lub
        /// "false" w przeciwnym przypadku
        /// </summary>
        /// <param name="conn">Połączenie z którego przyszły dane autoryzacyjne</param>
        /// <returns>True - autoryzacja poprawna; False - autoryzacja odrzucona</returns>
        protected abstract bool AuthorizeConnection(ClientConnection conn, JObject requestData);

        /// <summary>
        /// Metoda wywoływana po uzyskaniu strumienia danych ze 
        /// zautoryzowanego połączenia. Strumień danych jest konwertowany
        /// do obiektu JObject i przekazywany wraz z połączeniem.
        /// </summary>
        /// <param name="conn">Połączenie klienta</param>
        /// <param name="requestData">Dane przychodzące od klienta</param>
        /// <returns>Odpowiedź dla klienta w formacie JObject</returns>
        protected abstract JObject HandleRequest(ClientConnection conn, JObject requestData);
	}
}
