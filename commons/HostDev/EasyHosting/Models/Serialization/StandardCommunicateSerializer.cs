using EasyHosting.Meta;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyHosting.Models.Serialization {
    /// <summary>
    /// Serializator definiujący standardowy komunikat od serwera (dowolna odpowiedź lub wysyłana informacja)
    /// </summary>
    public class StandardCommunicateSerializer : BaseSerializer {
        public const string TYPE_RESPONSE = "REQUEST_RESPONSE";
        public const string TYPE_REQUEST_ERROR = "REQUEST_ERROR";
        public const string TYPE_LOBBY_SIGNAL = "LOBBY_SIGNAL";
        public const string TYPE_AUTHORIZATION = "AUTHORIZATION";
        public const string TYPE_SERVER_SIGNAL = "SERVER_SIGNAL";

        /// <summary>
        /// Określa typ komunikatu
        /// </summary>
        [SerializerField(apiName: "communicate-type", required: true)]
        public string CommunicateType;

        /// <summary>
        /// Jeżeli zapytania przychodzące refiniowało kod zapytania, powinien on zostać zwrotnie przekazany w komunikacie wychodzącym
        /// </summary>
        [SerializerField(apiName: "request-code", required: false, defaultValue: null)]
        public long RequestCode;

        /// <summary>
        /// Określa dane komunikatu
        /// </summary>
        [SerializerField(apiName: "data", required: true)]
        public JObject Data;

        public StandardCommunicateSerializer() : base() { }
        public StandardCommunicateSerializer(JObject data) : base(data) { }
    }
}
