using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Client {
    public class Request {
        public long RequestId { private set; get; }
        private ClientSocket _ParentSocket = null;
        /// <summary>
        /// Socket klienta, za pośrednictwem którego zostało wysłane zapytanie
        /// </summary>
        public ClientSocket ParentSocket {
            get {
                return _ParentSocket;
            }
        }

        public RequestState RequestState {
            get; protected set;
        }

        public JObject RequestData {
            get; protected set;
        }
        public JObject ResponseData {
            get; protected set;
        }

        private DateTime _SentAt;
        /// <summary>
        /// Określa moment czasowy, w którym zapytanie zostało wysłane. Jest ustawione tylko jeśli zapytanie ma status SENT lub RESPONSE_RECEIVED
        /// </summary>
        public DateTime SentAt {
            get { return _SentAt; }
        }
        private DateTime _ResponseReceivedAt;
        /// <summary>
        /// Określa moment czasowy, w którym przypisana została odpowiedź na wysłane zapytanie. Jest ustawione tylko jeżeli zapytanie ma status RESPONSE_RECEIVED
        /// </summary>
        public DateTime ResponseReceivedAt {
            get { return _ResponseReceivedAt; }
        }

        public Request(ClientSocket parentSocket, JObject requestData, long requestId) {
            _ParentSocket = parentSocket;
            RequestState = RequestState.CREATED;
            RequestData = requestData;
            ResponseData = null;
            RequestId = requestId;
        }

        /// <summary>
        /// Wysyła zapytanie do serwera
        /// </summary>
        /// <exception cref="RequestStateException">Rzucany, jeśli zapytanie zostało już wysłane i następuje ponowna próba wysłania zapytania</exception>
        public void Send() {
            if (RequestState == RequestState.CREATED) {
                RequestState = RequestState.SENT;
                _SentAt = DateTime.Now;
                ParentSocket.WriteData(RequestData);
                ParentSocket.TcpClient.GetStream().Flush();
            }
            else {
                throw new RequestStateException("Request has already been sent");
            }
        }
        /// <summary>
        /// Przypina odpowiedź dla zapytania
        /// </summary>
        /// <param name="response">Odpowiedź do przypięcia</param>
        /// <exception cref="RequestStateException">Rzucany, jeżeli zapytanie nie jest w stanie "SENT" przy próbie przypięcia odpowiedzi</exception>
        public void AttachResponse(JObject response) {
            if(RequestState == RequestState.SENT) {
                RequestState = RequestState.RESPONSE_RECEIVED;
                _ResponseReceivedAt = DateTime.Now;
                ResponseData = response;
            }
            else {
                throw new RequestStateException("Can only attach a response to the request if the reuqest state is SENT");
            }
        }
    }

    public enum RequestState {
        CREATED = 0,
        SENT = 1,
        RESPONSE_RECEIVED = 2
    };
}
