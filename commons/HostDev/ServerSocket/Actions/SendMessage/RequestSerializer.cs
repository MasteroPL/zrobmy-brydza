using EasyHosting.Meta;
using EasyHosting.Meta.Validators;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;

namespace ServerSocket.Actions.SendMessage {
    public class RequestSerializer : BaseSerializer {
        //[RangeValidator(minValue:1, maxValue:50)]
        [SerializerField(apiName:"message")]
        public string Message;

        public override void Validate(bool throwException = true) {
            base.Validate(throwException);

            if (Message.Length > 50)
            {
                AddError("Message", "INVALID_MESSAGE_LENGTH", "Długość wiadomości nie może przekraczać 50 znaków");
            }

            if (Errors.Count > 0 && throwException)
            {
                ThrowException();
            }
        }

        public RequestSerializer() : base() { }
        public RequestSerializer(JObject data) : base(data) { }
    }
}
