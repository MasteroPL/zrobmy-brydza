using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Serializers {
    public class LobbySignalUserRemovedSerializer : BaseSerializer {
        public const string SINGAL_USER_REMOVED = "USER_REMOVED";

        [SerializerField(apiName: "signal", required: true)]
        public string Signal;
        [SerializerField(apiName: "message", required: false, defaultValue: null)]
        public string Message;
        [SerializerField(apiName: "username", required: true)]
        public string Username;
        [SerializerField(apiName: "was_sitted", required:true)]
        public bool WasSitted;
        [SerializerField(apiName: "place_tag", required:false)]
        public int PlaceTag;

        public LobbySignalUserRemovedSerializer() : base() { }
        public LobbySignalUserRemovedSerializer(JObject data) : base(data) { }
    }
}
