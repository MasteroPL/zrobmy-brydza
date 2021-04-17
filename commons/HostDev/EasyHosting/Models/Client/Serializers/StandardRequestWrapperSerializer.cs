using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Client.Serializers {
    public class StandardRequestWrapperSerializer : BaseSerializer {
        /// <summary>
        /// Kod (identyfikator) zapytania, zwracany w odpowiedzi do zapytania od serwera
        /// </summary>
        [SerializerField(apiName: "request-code", required:false, defaultValue:null)]
        public long RequestCode;

        /// <summary>
        /// Dane zapytania
        /// </summary>
        [SerializerField(apiName: "data", required:true)]
        public JObject Data;

        public StandardRequestWrapperSerializer() : base() { }
        public StandardRequestWrapperSerializer(JObject data) : base(data) { }
    }
}
