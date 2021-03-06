﻿using GameManagerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code.UI;
using Debug = UnityEngine.Debug;

namespace Assets.Code.Models
{
    public class Game
    {
        public GameManagerScript GameManagerScript;
        public Match Match;

        public Game(GameManagerScript GameManagerScript)
        {
            this.GameManagerScript = GameManagerScript;
            Match = new Match(enableCardsShufflingAndDistributing:false);
            this.GameManagerScript.UpdateTableCenter(this);
        }

        public void StartGame()
        {
            if (!GameManagerScript.SeatManager.AllFourPlayersPresent())
            {
                return; // if there aren't 4 players to player the game cannot be started
            }

            /*Match.AddPlayer(new Player(PlayerTag.N, "gracz1"));
            Match.AddPlayer(new Player(PlayerTag.E, "gracz2"));
            Match.AddPlayer(new Player(PlayerTag.S, "gracz3"));
            Match.AddPlayer(new Player(PlayerTag.W, "gracz4"));*/

            //Match.Start();
            //GameState = GameState.BIDDING;
            //GameManagerScript.

            //Match.PlayerList[0].Hand, Match.PlayerList[1].Hand, Match.PlayerList[2].Hand, Match.PlayerList[3].Hand
            /*GameManagerLib.Models.Card[] MyHand = { };
            switch (UserData.Position)
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
            GameManagerScript.StartGame(this, MyHand);*/
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

        public bool PutCard(CardFigure Figure, CardColor Color, PlayerTag owner)
        {
            PlayerTag ownerPartner = (PlayerTag)(((int)owner + 2) % 4);
            if (UserData.Position != owner && UserData.Position != ownerPartner)
            {
                return false;
            }

            //Debug.Log("Trying to put " + Figure.ToString() + "_" + Color.ToString() + " possessed by " + owner.ToString() + ". You are sitting at position " + UserData.position.ToString());
            bool isOK = Match.CheckNextCard(owner, Color, Figure);
            if (isOK)
            {
                try
                {
                    Match.NextCard(owner, Color, Figure);
                    isOK = true;
                }
                catch(GameManagerLib.Exceptions.WrongCardException ex) // in case any exception is thrown
                {
                    isOK = false;
                }   
            }
            return isOK;
        }

        public bool IsTrickComplete()
        {
            return Match.CurrentGame.currentTrick.GetCount() == 0;
        }

        public int CalculateTeamTricks(PlayerTag player1, PlayerTag player2)
        {
            if (Math.Abs((int) player1 - (int)player2) != 2)
            {
                return -1;
            }

            int counter = 0;
            List<Trick> TrickList = Match.CurrentGame.TrickList;
            for (int i = 0; i < TrickList.Count; i++)
            {
                if (TrickList[i].Winner == player1 || TrickList[i].Winner == player2)
                {
                    counter++;
                }
            }
            return counter;
        }
    }
}
