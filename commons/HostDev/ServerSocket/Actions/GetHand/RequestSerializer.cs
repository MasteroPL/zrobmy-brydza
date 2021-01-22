using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using ServerSocket.Serializers;

namespace ServerSocket.Actions.GetHand
{
    class RequestSerializer : BaseSerializer
    {
        [SerializerField(apiName: "cards")]
        public CardSerializer[] Hand;
        [SerializerField(apiName: "player-ID")]
        public int PlayerID;

        public override void Validate(bool throwException = true)
        {
            base.Validate(throwException);
            foreach (CardSerializer card in Hand)
            {
                card.Validate();
            }

            if (Errors.Count == 0)
            {
                if (Hand.Length != 13)
                {
                    AddError("cards", "INVALID_NUMBER_OF_CARDS", "Musi być 13 kart.");
                }
                
                if(PlayerID < -1 || PlayerID > 3)
                {
                    AddError("PlayerID", "INVALID_PLAYER_ID", "Dozwolone wartości <-1,3>.");
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
}
