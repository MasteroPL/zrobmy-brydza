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
    public class PutCardSignalSerializer : BaseSerializer
    {
        public const string SIGNAL_USER_PUT_CARD = "USER_PUT_CARD";

        [SerializerField(apiName: "signal", required: true)]
        public string Signal;
        [SerializerField(apiName: "message", required: false, defaultValue: null)]
        public string Message;
        [SerializerField(apiName: "username", required: true)]
        public string Username;
        [SerializerField(apiName: "figure", required: true)]
        public int CardFigure;
        [SerializerField(apiName: "color", required: true)]
        public int CardColor;

        public PutCardSignalSerializer() : base() { }
        public PutCardSignalSerializer(JObject data) : base(data) { }
    }
}
