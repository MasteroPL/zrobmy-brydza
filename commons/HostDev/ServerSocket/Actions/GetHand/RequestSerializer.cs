using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using ServerSocket.Serializers;

namespace ServerSocket.Actions.GetHand
{
    public class RequestSerializer : BaseSerializer
    {
        [SerializerField(apiName: "player_tag")]
        public int PlayerTag;

        public override void Validate(bool throwException = true)
        {
            base.Validate(throwException);

            if(PlayerTag < 0 || PlayerTag > 3) {
                AddError("PlayerTag", "INVALID_TAG", "Niepoprawny tag gracza");
            }
        }

        public RequestSerializer() : base() { }
        public RequestSerializer(JObject data) : base(data) { }
    }
}