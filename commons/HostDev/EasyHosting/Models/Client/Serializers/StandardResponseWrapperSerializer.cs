using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Client.Serializers {
    public class StandardResponseWrapperSerializer : BaseSerializer {
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

        public StandardResponseWrapperSerializer() : base() { }
        public StandardResponseWrapperSerializer(JObject data) : base(data) { }
    }
}
