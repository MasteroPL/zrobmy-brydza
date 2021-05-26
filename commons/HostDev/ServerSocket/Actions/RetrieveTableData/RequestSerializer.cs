using EasyHosting.Meta;
using EasyHosting.Meta.Validators;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;

namespace ServerSocket.Actions.RetrieveTableData
{
    public class RequestSerializer : BaseSerializer
    {
        public override void Validate(bool throwException = true)
        {
            base.Validate(throwException);
        }

        public RequestSerializer() : base() { }
        public RequestSerializer(JObject data) : base(data) { }
    }
}
