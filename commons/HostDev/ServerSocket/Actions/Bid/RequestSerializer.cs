using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using ServerSocket.Serializers;

namespace ServerSocket.Actions.Bid
{
    public class RequestSerializer : BaseSerializer
    {
        [SerializerField(apiName: "height")]
        public int Height;
        [SerializerField(apiName: "color")]
        public int Color;
        // Double - kontra
        [SerializerField(apiName: "X")]
        public bool X;
        // redouble - rekontra
        [SerializerField(apiName: "XX")]
        public bool XX;
        public override void Validate(bool throwException = true)
        {
            base.Validate(throwException);

            if (!X && !XX ) {
                if (Height < -1 || Height > 7 || Height == 0)
                {
                    AddError("Height", "INVALID_CONTRACT_HEIGHT", "Za niski lub zawisoki kontrakt. Dozwolone wartości <-1> ∪ <1,7>");
                }

                if (Color < -1 || Color > 4)
                {
                    AddError("Color", "INVALID_COLOR", "Zły kolor, dozwolone wartości <0,4>");
                }
            }

            if (Errors.Count > 0 && throwException)
            {
                ThrowException();
            }
        }
}
