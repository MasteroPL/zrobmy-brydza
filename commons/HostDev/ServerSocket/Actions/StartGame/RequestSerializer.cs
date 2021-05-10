using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;

namespace ServerSocket.Actions.StartGame {
    public class RequestSerializer : BaseSerializer {
        [SerializerField(apiName: "place_tag", required: true)]
        public int PlaceTag;
        [SerializerField(apiName: "username", required: true)]
        public string Username;

        public override void Validate(bool throwException = true)
        {
            base.Validate(throwException);

            if (Errors.Count == 0)
            {
                if (PlaceTag > 3 || PlaceTag < 0)
                {
                    AddError("PlaceTag", "INVALID_PLACE", "Niepoprawny kod miejsca. Dostępny zakres: 0-3");
                }
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
