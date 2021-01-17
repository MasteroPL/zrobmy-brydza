using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;

namespace ServerSocket.Actions.StartGame {
    public class RequestSerializer : BaseSerializer {

        public RequestSerializer() : base() { }
        public RequestSerializer(JObject data) : base(data) { }
    }
}
