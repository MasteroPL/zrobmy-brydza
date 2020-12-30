using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTesting {
    public class RequestSerializer : BaseSerializer {
        // Na wejściu będziemy oczekiwać imienia
        [SerializerField("my_name", required:true)]
        public string MyName;

        public RequestSerializer() : base() { }
        public RequestSerializer(JObject data) : base(data) { }
    }
}
