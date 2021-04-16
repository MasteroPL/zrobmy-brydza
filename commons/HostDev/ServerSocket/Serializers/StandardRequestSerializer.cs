using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Serializers {
    /// <summary>
    /// Serializator do inicjalnej walidacji danych przychodzących (przed przekazaniem do menadżera akcji)
    /// </summary>
    public class StandardRequestSerializer : BaseSerializer {
        /// <summary>
        /// Jeżeli zapytanie przychodzące definiuje kod zapytania, zostanie on zwrócony w odpowiedzi (wyniku zapytania)
        /// </summary>
        [SerializerField(apiName: "request-code", required: false, defaultValue: null)]
        public long RequestCode;

        /// <summary>
        /// Dane zapytania
        /// </summary>
        [SerializerField(apiName: "data", required: true)]
        public JObject Data;

        public StandardRequestSerializer() : base() { }
        public StandardRequestSerializer(JObject data) : base(data) { }
    }
}
