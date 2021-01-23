using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Serializers {
    public class PlayerSerializer : BaseSerializer {
        [SerializerField(apiName:"username")]
        public string Username;

        [SerializerField(apiName:"player_tag")]
        public int PlayerTag;

        public PlayerSerializer() : base() { }
        public PlayerSerializer(JObject data) : base(data) { }
    }
}
