using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Serializers {
    public class LobbySignalUserSatSerializer : BaseSerializer {
        public const string SIGNAL_USER_SAT = "USER_SAT";

        [SerializerField(apiName: "signal", required: true)]
        public string Signal;
        [SerializerField(apiName: "place_tag", required: true)]
        public int PlaceTag;
        [SerializerField(apiName: "username", required: true)]
        public string Username;

        public override void Validate(bool throwException = true) {
            base.Validate(throwException);

            if (Errors.Count == 0) {
                if (PlaceTag > 3 || PlaceTag < 0) {
                    AddError("PlaceTag", "INVALID_PLACE", "Niepoprawny kod miejsca. Dostępny zakres: 0-3");
                }
            }

            if (Errors.Count > 0 && throwException) {
                ThrowException();
            }
        }

        public LobbySignalUserSatSerializer() : base() { }
        public LobbySignalUserSatSerializer(JObject data) : base(data) { }
    }
}
