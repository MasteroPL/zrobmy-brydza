using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Serializers {
    public class LobbyUserSerializer : BaseSerializer {

        [SerializerField(apiName:"username")]
        public string Username;

        [SerializerField(apiName:"is_sitted")]
        public bool IsSitted;

        [SerializerField(apiName:"player_tag")]
        public int PlayerTag;

        public LobbyUserSerializer() : base() { }
        public LobbyUserSerializer(JObject data) : base(data) { }
    }
}
