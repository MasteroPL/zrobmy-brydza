using GameManagerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Models
{
    public class Game
    {
        public GameManagerScript GameManagerScript;
        public GameState GameState;

        public Game()
        {
            GameManagerScript = new GameManagerScript();
            GameState = GameState.AWAITING_PLAYERS;
        }

        public void StartGame()
        {
            Match match = new Match();
            match.AddPlayer(new Player(PlayerTag.N, "gracz1"));
            match.AddPlayer(new Player(PlayerTag.E, "gracz2"));
            match.AddPlayer(new Player(PlayerTag.S, "gracz3"));
            match.AddPlayer(new Player(PlayerTag.W, "gracz4"));

            match.Start();
            GameState = GameState.BIDDING;
            GameManagerScript.StartGame(match.PlayerList[0].Hand, match.PlayerList[1].Hand, match.PlayerList[2].Hand, match.PlayerList[3].Hand);
        }
    }
}
