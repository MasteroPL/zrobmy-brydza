using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Actions.PutCard
{
    public class ResponseSerializer : BaseSerializer
    {
        [SerializerField(apiName: "status")]
        public string Status;
        [SerializerField(apiName: "owner_position")]
        public int OwnerPosition;
        [SerializerField(apiName: "figure")]
        public int CardFigure;
        [SerializerField(apiName: "color")]
        public int CardColor;

        public ResponseSerializer() : base() { }
        public ResponseSerializer(JObject data) : base(data) { }
    }
}
