using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Server.Serializers {
    public class StandardResponseSerializer : BaseSerializer {

        [SerializerField(apiName:"status")]
        public string Status;
        [SerializerField(apiName:"message")]
        public string Message;

        public StandardResponseSerializer() : base() { }
        public StandardResponseSerializer(JObject data) : base(data) { }
    }
}
