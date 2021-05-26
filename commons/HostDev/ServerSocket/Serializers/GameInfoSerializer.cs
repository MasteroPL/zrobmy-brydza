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
    public class GameInfoSerializer : BaseSerializer
    {
        [SerializerField(apiName: "current_player")]
        public int CurrentPlayer;
        [SerializerField(apiName: "declarer")]
        public int Declarer;
        [SerializerField(apiName: "contract_color")]
        public int ContractColor;
        [SerializerField(apiName: "current_trick")]
        public TrickSerializer CurrentTrick;

        public GameInfoSerializer() : base() { }
        public GameInfoSerializer(JObject data) : base(data) { }
    }
}
