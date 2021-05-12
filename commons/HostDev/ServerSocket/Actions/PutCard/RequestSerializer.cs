using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using GameManagerLib.Models;

namespace ServerSocket.Actions.PutCard
{
    public class RequestSerializer : BaseSerializer
    {
        [SerializerField(apiName: "owner_position")]
        public int CardOwnerPosition;

        [SerializerField(apiName: "figure")]
        public int Figure;

        [SerializerField(apiName: "color")]
        public int Color;

        public override void Validate(bool throwException = true)
        {
            base.Validate(throwException);

            if ((CardFigure)Figure < CardFigure.TWO || (CardFigure)Figure > CardFigure.ACE)
            {
                AddError("Figure", "INVALID_CARD_HEIGHT", "Podano nieistniejącą wysokość karty");
            }
            if ((CardColor)Color < CardColor.CLUB || (CardColor)Color > CardColor.SPADE)
            {
                AddError("Color", "INVALID_CARD_COLOR", "Podano nieistniejący kolor karty");
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
