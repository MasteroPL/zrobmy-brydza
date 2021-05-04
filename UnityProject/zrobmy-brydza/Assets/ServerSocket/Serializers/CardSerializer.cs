using System;
using System.Globalization;
using System.IO;

using EasyHosting.Meta;
using EasyHosting.Models.Serialization;

using Newtonsoft.Json.Linq;

namespace ServerSocket.Serializers
{
    public class CardSerializer : BaseSerializer
    {
        [SerializerField(apiName: "color")]
        public int Color;
        [SerializerField(apiName: "figure")]
        public int Figure;
        [SerializerField(apiName: "state")]
        public int State;

        public override void Validate(bool throwException = true)
        {
            base.Validate(throwException);

            if (Errors.Count == 0)
            {
                if (Color < 0 || Color > 3)
                {
                    AddError("Color", "INVALID_CARD_COLOR", "Niepoprawny kolor karty. Dostępne wartość <0,3> 0 = CLUB, 1 = DIAMOND, 2 = HEART, 3 = SPADE.");
                }

                if (Figure < 2 || Figure > 14)
                {
                    AddError("Figure", "INVALID_CARD_FIGURE", "Niepoprawna figura karty. Dostępny zakres <2,14>.");
                }

                if(State < 0 || State > 3)
                {
                    AddError("State", "INVALID_STATE", "Nieporawny stan karty. Dostępny zakreś <0,3>.");
                }
            }

            if (Errors.Count > 0 && throwException)
            {
                ThrowException();
            }
        }
        public CardSerializer() : base() { }
        public CardSerializer(JObject data) : base(data) { }
    }
}
