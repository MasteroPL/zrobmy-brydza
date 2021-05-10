using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using ServerSocket.Serializers;

namespace ServerSocket.Actions.GetHand
{
    public class RequestSerializer : BaseSerializer
    {
        [SerializerField(apiName: "player-ID")]
        public int PlayerID;

        [SerializerField(apiName: "username")]
        public string Username;

        public override void Validate(bool throwException = true)
        {
            base.Validate(throwException);

            if (PlayerID == -1)
            {
                ThrowException();
            }
        }

        public RequestSerializer() : base() { }
        public RequestSerializer(JObject data) : base(data) { }
    }
}