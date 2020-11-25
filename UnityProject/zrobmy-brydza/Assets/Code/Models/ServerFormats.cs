using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Models
{
    [Serializable]
    public class ServerResponse
    {
        public List<CardObject> PlayerHand;

        public ServerResponse(List<CardObject> PlayerHand)
        {
            this.PlayerHand = PlayerHand;
        }
    }

    [Serializable]
    public class ServerRequest
    {
        public CardObject PutCard;
        public PlayerTag Position;

        public ServerRequest(CardObject Card, PlayerTag Position)
        {
            PutCard = Card;
            this.Position = Position;
        }
    }
}
