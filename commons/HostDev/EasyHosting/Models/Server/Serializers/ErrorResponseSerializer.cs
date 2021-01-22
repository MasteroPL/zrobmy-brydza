using EasyHosting.Meta;
using EasyHosting.Meta.Validators;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Server.Serializers {
    public class ErrorResponseSerializer : BaseSerializer {

        [SerializerField(apiName:"status")]
        public string Status;

        [SerializerField(apiName:"errors")]
        public Dictionary<string, List<ValidationError>> FieldErrors;

        public ErrorResponseSerializer() : base() { }
        public ErrorResponseSerializer(JObject data) : base(data) { }
    }
}
