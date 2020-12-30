using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Serializers {
    public class AuthorizationSerializer : BaseSerializer {

        [SerializerField(apiName: "lobby-id", required: false, defaultValue: "DEFAULT")]
        public string LobbyId;

        [SerializerField(apiName: "login")]
        public string Login;

        [SerializerField(apiName: "lobby-password")]
        public string LobbyPassword;

        public AuthorizationSerializer() : base() { }
        public AuthorizationSerializer(JObject data) : base(data) { }
    }
}
