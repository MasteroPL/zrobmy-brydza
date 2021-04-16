using System;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Net;
using EasyHosting.Meta.Validators;
using EasyHosting.Models.Server.Serializers;

namespace EasyHosting.Models.Server
{
    /// <summary>
    /// Klasa definiująca podstawowe funkcjonalności gniazda serwera, tj. przechwytywanie połączeń, rozpoznawanie połączeń zautoryzowanych, wymaganie autoryzacji. Wymaga nadpisania AuthorizeConnection oraz HandleRequest
    /// </summary>
    public abstract class ServerSocket {
        private List<ClientConnection> UnauthorizedConnections = new List<ClientConnection>();
        private List<ClientConnection> AuthorizedConnections = new List<ClientConnection>();

        private List<ClientConnection> ClientsToDisconnect = new List<ClientConnection>();

        /// <summary>
        /// Określa po jakim czasie bez poprawnej autoryzacji połączenie z klientem zostanie zamknięte przez gniazdo
        /// </summary>
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
        /// <summary>
        /// Określa na jakim adresie IP nasłuchuje gniazdo
        /// </summary>
        public System.Net.IPAddress IpAddress { get { return _IpAddress; } private set { _IpAddress = value; } }

        private int _Port = 33564;
        /// <summary>
        /// Określa port na którym nasłuchuje gniazdo
        /// </summary>
        public int Port { get { return _Port; } private set { _Port = value; } }

        /// <summary>
        /// Standardowa odpowiedź od serwera po poprawnej autoryzacji
        /// </summary>
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
                        JObject response;

                        try {
                            try {
                                response = HandleRequest(connection, data);
                            } catch (ValidationException e) {
                                response = e.GetJson();
                            }
                        } catch (Exception e) {
                            var resp = new StandardResponseSerializer() {
                                Status = "ERR_INTERNAL",
                                Message = "Wystąpił wewnętrzny błąd serwera"
                            };
                            response = resp.GetApiObject();
                        }

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
                            // Zero zaufania do niezautoryzowanych połączeń
                            try {
                                JObject data = connection.GetData();
                                if (AuthorizeConnection(connection, data)) {
                                    AuthorizedConnections.Add(connection);
                                    toRemove.Add(connection);

                                    var response = GetAuthorizationResponseSuccessful();

                                    // Informacja dla odbiorcy o poprawnej autoryzacji
                                    connection.WriteData(response);
                                    connection.Flush();
                                }
                                else {
                                    var response = GetAuthorizationResponseFailed();
                                    connection.WriteData(response);
                                    connection.Flush();
                                }
                            }catch(Exception e) {
                                connection.WriteData(GetAuthorizationResponseFailed());
                                connection.Flush();
                            }
                        }
                    } 
                    else {
                        connection.WriteData(GetAuthorizationTimeoutSignal());
                        connection.Flush();

                        connection.TcpClient.Close();
                        toRemove.Add(connection);
                    }
                }

                // Finalizacja przenoszenie połączenia po zautoryzowaniu lub zamknięciu
                foreach (var connection in toRemove) {
                    UnauthorizedConnections.Remove(connection);
                }

                // Odłączanie klientów do odłączenia
                foreach(var connection in ClientsToDisconnect) {
                    if (AuthorizedConnections.Contains(connection)) {
                        connection.WriteData(GetDisconnectedSignal());
                        connection.Flush();

                        connection.TcpClient.Close();
                        AuthorizedConnections.Remove(connection);
                    }
                    else if (UnauthorizedConnections.Contains(connection)) {
                        connection.WriteData(GetDisconnectedSignal());
                        connection.Flush();

                        connection.TcpClient.Close();
                        UnauthorizedConnections.Remove(connection);
                    }
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
        /// <summary>
        /// Określa jaka odpowiedź ma być zwrócona do klienta w przypadku udanej autoryzacji
        /// </summary>
        /// <returns>Obiekt JSON do przekazania do klienta</returns>
        protected virtual JObject GetAuthorizationResponseSuccessful() {
            return AuthorizationSuccessfulResponse;
        }
        /// <summary>
        /// Określa jaka odpowiedź ma być zwrócona do klienta w przypadku nieudanej autoryzacji
        /// </summary>
        /// <returns>Obiekt JSON do przekazania do klienta</returns>
        protected virtual JObject GetAuthorizationResponseFailed() {
            var resp = new StandardResponseSerializer() {
                Status = "FORBIDDEN",
                Message = "Authorization failed"
            };
            return resp.GetApiObject();
        }
        /// <summary>
        /// Treść komunikatu przy odłączeniu klienta od serwera
        /// </summary>
        /// <returns>Obiekt JSON do przekazania do klienta</returns>
        protected virtual JObject GetDisconnectedSignal() {
            var resp = new StandardResponseSerializer() {
                Status = "DISCONNECTED",
                Message = "You have been disconnected"
            };
            return resp.GetApiObject();
        }
        /// <summary>
        /// Treść komunikatu przy odłączeniu klienta od serwera przez zbyt długi czas autoryzacji
        /// </summary>
        /// <returns>Obiekt JSON do przekazania do klienta</returns>
        protected virtual JObject GetAuthorizationTimeoutSignal() {
            var resp = new StandardResponseSerializer() {
                Status = "AUTHORIZATION_TIMEOUT",
                Message = "Authorization failed - timeout"
            };
            return resp.GetApiObject();
        }

        /// <summary>
        /// Uruchamia socket
        /// </summary>
        public void Start() {
            Listen();
        }

        public void StartInThread() {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Odłącza klienta od serwera
        /// </summary>
        /// <param name="clientToDisconnect">Klient do odłączenia</param>
        /// <returns>True jeśli odłączenie poprawne, False jeśli klient już dodany do listy klientów do odłączenia</returns>
        /// <exception cref="ArgumentException">Rzucany jeśli klient nie jest połączony z serwerem</exception>
        public bool DisconnectClient(ClientConnection clientToDisconnect) {
            if (ClientsToDisconnect.Contains(clientToDisconnect)) {
                return false;
            }

            else if (AuthorizedConnections.Contains(clientToDisconnect)) {
                ClientsToDisconnect.Add(clientToDisconnect);
                return true;
            }

            else if (UnauthorizedConnections.Contains(clientToDisconnect)) {
                ClientsToDisconnect.Add(clientToDisconnect);
                return true;
            }

            else {
                throw new ArgumentException("Client not connected");
            }
        }
        /// <summary>
        /// Sprawdza, czy klient jest połączony z serwerem
        /// </summary>
        /// <param name="client">Klient do sprawdzenia</param>
        /// <param name="searchDependingOnStatus">Jeśli true, użyty zostanie dodatkowy filtr, sprwadzający tylko klientów zautoryzowanych lub tylko niezautoryzacowanych</param>
        /// <param name="connectionStatus">Jeśli searchDependingOnStatus = true, po jakim statusie powinniśmy wyszukiwać połączenia</param>
        /// <returns></returns>
        public bool ClientConnected(ClientConnection client, bool searchDependingOnStatus = false, ConnectionStatus connectionStatus = ConnectionStatus.ANY) {
            if(!searchDependingOnStatus || connectionStatus == ConnectionStatus.ANY) {
                if (AuthorizedConnections.Contains(client)) {
                    return true;
                }
                if (UnauthorizedConnections.Contains(client)) {
                    return true;
                }
            }
            else {
                if(connectionStatus == ConnectionStatus.AUTHORIZED) {
                    if (AuthorizedConnections.Contains(client)) {
                        return true;
                    }
                }
                else if(connectionStatus == ConnectionStatus.UNAUTHORIZED) {
                    if (UnauthorizedConnections.Contains(client)) {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Metoda wywoływana po uzyskaniu pierwszego strumienia danych z 
        /// niezautoryzowanego połączenia. Powinna zwalidować poprawność 
        /// danych autoryzacyjnych w przychodzącym strumieniu danych
        /// i zwrócić "true" jeśli autoryzacja przebiegła pomyslnie lub
        /// "false" w przeciwnym przypadku
        /// </summary>
        /// <param name="conn">Połączenie z którego przyszły dane autoryzacyjne</param>
        /// <param name="requestData">Dane przychodzące od klienta</param>
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

    public enum ConnectionStatus {
        ANY = -1,
        UNAUTHORIZED = 0,
        AUTHORIZED = 1
    };
}
