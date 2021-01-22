using EasyHosting.Meta;
using EasyHosting.Meta.Validators;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;

namespace ServerSocket.Actions.SendChat {
    public class RequestSerializer : BaseSerializer {

        [RangeValidator(minValue:1, maxValue:50)]
        [SerializerField(apiName:"message")]
        public string Message;

        public override void Validate(bool throwException = true) {
            base.Validate(throwException);
        }

        public RequestSerializer() : base() { }
        public RequestSerializer(JObject data) : base(data) { }
    }
}
