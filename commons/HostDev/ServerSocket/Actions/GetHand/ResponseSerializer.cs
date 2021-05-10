using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using GameManagerLib.Models;
using System.Collections.Generic;
using ServerSocket.Serializers;

namespace ServerSocket.Actions.GetHand
{
    public class ResponseSerializer : BaseSerializer
    {
        [SerializerField(apiName: "status", required: true)]
        public string Status;

        [SerializerField(apiName: "cards", required: true)]
        public CardSerializer[] Cards;

        public override void Validate(bool throwException = true)
        {
            base.Validate(throwException);
            foreach (CardSerializer card in Cards)
            {
                card.Validate();
            }

            if (Errors.Count == 0)
            {
                if (Cards.Length != 13)
                {
                    AddError("cards", "INVALID_NUMBER_OF_CARDS", "Musi być 13 kart.");
                }
            }
        }

        public ResponseSerializer() : base() { }
        public ResponseSerializer(JObject data) : base(data) { }
    }
}
