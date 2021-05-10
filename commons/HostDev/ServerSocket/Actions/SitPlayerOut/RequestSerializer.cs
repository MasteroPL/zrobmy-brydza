using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Actions.SitPlayerOut {
    public class RequestSerializer : BaseSerializer {
        [SerializerField(apiName: "place_tag")]
        public int PlaceTag;

        public override void Validate(bool throwException = true) {
            base.Validate(throwException);

            if (Errors.Count == 0) {
                if (PlaceTag > 3 || PlaceTag < 0) {
                    AddError("place_tag", "INVALID_PLACE", "Niepoprawny kod miejsca. Dostępny zakres: 0-3");
                }
            }

            if (Errors.Count > 0 && throwException) {
                ThrowException();
            }
        }

        public RequestSerializer() : base() { }
        public RequestSerializer(JObject data) : base(data) { }
    }
}
