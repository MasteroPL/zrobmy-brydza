using GameManagerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BidActionRequestSerializer = ServerSocket.Actions.Bid.RequestSerializer;
using StartGameActionRequestSerializer = ServerSocket.Actions.StartGame.RequestSerializer;

namespace ClientSocketTesting {
    public class DumbAIMarcin2_0 : DumbAI {
        public override void Play() {
            base.Play();

            if(Game.GameState == GameState.STARTING) {
                var data = new StartGameActionRequestSerializer() {
                    Username = Username,
                    PlaceTag = (int)Position
                };
                PerformServerAction("start-game", data.GetApiObject(), null, null);
            }
        }

        protected override void PlayBidding() {
            if (Game.CurrentBidding.CurrentPlayer == Position) {
                var data = new BidActionRequestSerializer() {
                    Height = (int)ContractHeight.THREE,
                    Color = (int)ContractColor.S,
                    X = false,
                    XX = false
                };

                Console.WriteLine(Username + "> Licytuje: 3S");
                PerformServerAction("bid", data.GetApiObject(), null, null);
            }
        }
    }
}
