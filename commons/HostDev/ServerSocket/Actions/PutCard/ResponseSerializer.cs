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

        public ResponseSerializer() : base() { }
        public ResponseSerializer(JObject data) : base(data) { }
    }
}
