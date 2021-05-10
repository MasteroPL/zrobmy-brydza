using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;

namespace ServerSocket.Serializers
{
    public class PlayerClickedGameStartSerializer : BaseSerializer
    {
        public const string SIGNAL_PLAYER_READY = "USER_READY_FOR_GAME";

        [SerializerField(apiName: "signal", required: true)]
        public string Signal;
        [SerializerField(apiName: "message", required: false, defaultValue: null)]
        public string Message;
        [SerializerField(apiName: "username", required: true)]
        public string Username;
        [SerializerField(apiName: "place_tag", required: true)]
        public int PlaceTag;

        public PlayerClickedGameStartSerializer() : base() { }
        public PlayerClickedGameStartSerializer(JObject data) : base(data) { }
    }
}
