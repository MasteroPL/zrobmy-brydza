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
    public class TrickSerializer : BaseSerializer
    {
        [SerializerField(apiName: "winner")]
        public int Winner;
        [SerializerField(apiName: "card_list")]
        public CardSerializer[] CardList;

        public TrickSerializer() : base() { }
        public TrickSerializer(JObject data) : base(data) { }
    }
}
