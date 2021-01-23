using GameManagerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code.UI;

namespace Assets.Code.Models
{
    public class Game
    {
        public GameManagerScript GameManagerScript;
        public GameState GameState;
        public Match Match;
        public UserData UserData;

        public Game(GameManagerScript GameManagerScript)
        {
            this.GameManagerScript = GameManagerScript;
            GameState = GameState.AWAITING_PLAYERS;
            Match = new Match();
            UserData = new UserData();
            this.GameManagerScript.UpdateTableCenter(this);
        }

        public void StartGame()
        {
            Match.AddPlayer(new Player(PlayerTag.N, "gracz1"));
            Match.AddPlayer(new Player(PlayerTag.E, "gracz2"));
            Match.AddPlayer(new Player(PlayerTag.S, "gracz3"));
            Match.AddPlayer(new Player(PlayerTag.W, "gracz4"));

            Match.Start();
            GameState = GameState.BIDDING;

            //Match.PlayerList[0].Hand, Match.PlayerList[1].Hand, Match.PlayerList[2].Hand, Match.PlayerList[3].Hand
            GameManagerLib.Models.Card[] MyHand = { };
            switch (UserData.position)
            {
                case PlayerTag.N:
                    MyHand = Match.PlayerList[0].Hand;
                    break;
                case PlayerTag.E:
                    MyHand = Match.PlayerList[1].Hand;
                    break;
                case PlayerTag.S:
                    MyHand = Match.PlayerList[2].Hand;
                    break;
                case PlayerTag.W:
                    MyHand = Match.PlayerList[3].Hand;
                    break;
            }
            GameManagerScript.StartGame(this, MyHand);
        }

        public void ShuffleAndGiveCardsAgain()
        {
            GameManagerLib.Models.Card[] MyHand = { };
            switch (UserData.position)
            {
                case PlayerTag.N:
                    MyHand = Match.PlayerList[0].Hand;
                    break;
                case PlayerTag.E:
                    MyHand = Match.PlayerList[1].Hand;
                    break;
                case PlayerTag.S:
                    MyHand = Match.PlayerList[2].Hand;
                    break;
                case PlayerTag.W:
                    MyHand = Match.PlayerList[3].Hand;
                    break;
            }
            GameManagerScript.RestartGame();
        }

        public bool AddBid(ContractHeight Height, ContractColor Color, PlayerTag Declarer, bool XEnabled=false, bool XXEnabled=false)
        {
            try
            {
                Match.AddBid(new Contract(Height, Color, Declarer, XEnabled, XXEnabled));
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public void ShowGrandCards()
        {
            PlayerTag grand = (PlayerTag)(((int)Match.CurrentGame.Declarer + 2) % 4);
            GameManagerLib.Models.Card[] grandCards = Match.PlayerList[(int)grand].Hand;
            GameManagerScript.ShowGrandCards(grand, grandCards);
        }

        public bool PutCard(GameManagerLib.Models.Card Card)
        {
            PlayerTag owner = Card.PlayerID;
            PlayerTag ownerPartner = (PlayerTag)(((int)Card.PlayerID + 2) % 4);
            if (UserData.position != Card.PlayerID && UserData.position != ownerPartner)
            {
                return false;
            }
            bool isOK = true;
            try
            {
                isOK = Match.NextCard(Card);
            }catch(GameManagerLib.Exceptions.WrongGameStateException ex)
            {
                Console.WriteLine("WrongGameState");
                isOK = false;
            }catch(GameManagerLib.Exceptions.WrongCardException ex)
            {
                Console.WriteLine("WrongCardException");
                isOK = false;
            }catch(GameManagerLib.Exceptions.UnexpectedFunctionEndException ex)
            {
                Console.WriteLine("UnexpectedFunctionEndException");
                isOK = false;
            }
            return isOK;
        }
    }
}
