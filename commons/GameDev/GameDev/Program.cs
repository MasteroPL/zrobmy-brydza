using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using GameManagerLib.Models;
using GameManagerLib.Exceptions;

namespace GameDev
{
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");
            Match match = new Match();
            match.AddPlayer(new Player((PlayerTag)(0), "gracz1"));//N
            match.AddPlayer(new Player((PlayerTag)(1), "gracz2"));//E
            match.AddPlayer(new Player((PlayerTag)(2), "gracz3"));//S
            match.AddPlayer(new Player((PlayerTag)(3), "gracz4"));//W

            match.Start();
            Console.WriteLine(match.CurrentBidding.CurrentPlayer.ToString());

            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(0))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(1))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(2))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(3))); //pas
            Console.WriteLine(match.CurrentBidding.CurrentPlayer.ToString());

            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(1))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(2))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(3))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(0))); //pas


            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(2))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(3))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(0))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(1))); //pas

            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(3))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(0))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(1))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(2))); //pas


            match.AddBid(new Contract((ContractHeight)(1), (ContractColor)(2), (PlayerTag)(0))); //1h
            match.AddBid(new Contract((ContractHeight)(-1),(ContractColor)(-1), (PlayerTag)(1))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(2))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(3))); //pas


            Console.WriteLine(match.CurrentBidding.CurrentPlayer.ToString());
            Console.WriteLine(match.GameState);

            match.NextCard(PlayerTag.N,CardColor.CLUB,CardFigure.ACE);
           
            Console.WriteLine(match.GameState);
            Console.WriteLine(match.GameState);

        }
    }
}
