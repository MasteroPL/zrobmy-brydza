using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTesting {
    public class ResponseSerializer : BaseSerializer {
        // Na wyjściu damy "Hello World"
        [SerializerField("hello_world")]
        public string HelloWorld;

        public ResponseSerializer() : base() { }
        public ResponseSerializer(JObject data) : base(data) { }
    }
}
