using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Serializers {
    public class LobbySignalNewBidSerializer : BaseSerializer {
        public const string SIGNAL_NEW_BID = "NEW_BID";

        [SerializerField(apiName: "signal", required: true)]
        public string Signal;
        [SerializerField(apiName: "username", required: true)]
        public string Username;
        [SerializerField(apiName: "place_tag", required: false)]
        public int PlaceTag;

        [SerializerField(apiName: "height")]
        public int Height;
        [SerializerField(apiName: "color")]
        public int Color;
        // Double - kontra
        [SerializerField(apiName: "X")]
        public bool X;
        // redouble - rekontra
        [SerializerField(apiName: "XX")]
        public bool XX;

        public LobbySignalNewBidSerializer() : base() { }
        public LobbySignalNewBidSerializer(JObject data) : base(data) { }
    }
}
