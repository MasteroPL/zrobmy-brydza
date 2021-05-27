using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Serializers {
    public class LobbySignalGameFinishedSerializer : BaseSerializer {
        public const string SIGNAL_GAME_FINISHED = "GAME_FINISHED";

        [SerializerField(apiName: "signal", required: true)]
        public string Signal;

        /// <summary>
        /// 0: Para NS
        /// 1: Para WE
        /// -1: Brak
        /// </summary>
        [SerializerField(apiName: "winner", required: true)]
        public short Winner;

        [SerializerField(apiName: "points_ns", required: true)]
        public int PointsNS;

        [SerializerField(apiName: "points_we", required: true)]
        public int PointsWE;

        [SerializerField(apiName: "rounds_ns", required: true)]
        public int RoundsNS;

        [SerializerField(apiName: "rounds_we", required: true)]
        public int RoundsWE;

        public LobbySignalGameFinishedSerializer() : base() { }
        public LobbySignalGameFinishedSerializer(JObject data) : base(data) { }
    }
}
