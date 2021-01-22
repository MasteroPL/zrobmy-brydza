using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;

namespace ServerSocket.Actions.LeaveTable
{
    public class RequestSerializer : BaseSerializer
    {
        [SerializerField(apiName: "player")]
        public string Player;
        public override void Validate(bool throwException = true)
        {
            base.Validate(throwException);

            if (Errors.Count > 0 && throwException)
            {
                ThrowException();
            }
        }

        public RequestSerializer() : base() { }
        public RequestSerializer(JObject data) : base(data) { }
    }
}
