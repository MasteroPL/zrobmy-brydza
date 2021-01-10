using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using GameManagerLib.Models;
using GameManagerLib.Exceptions;
namespace GameManagerLib
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

            match.AddBid(new Contract((ContractHeight)(1), (ContractColor)(2), (PlayerTag)(0))); //1h
            match.AddBid(new Contract((ContractHeight)(-1),(ContractColor)(-1), (PlayerTag)(1))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(2))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(3))); //pas


            Console.WriteLine(match.CurrentBidding.CurrentPlayer.ToString());
            Console.WriteLine(match.GameState);

            match.NextCard(match.PlayerList[1].Hand[0]);
            match.NextCard(match.PlayerList[2].Hand[3]);
            match.NextCard(match.PlayerList[3].Hand[2]);
            match.NextCard(match.PlayerList[0].Hand[1]);

            match.NextCard(match.PlayerList[2].Hand[0]);
            match.NextCard(match.PlayerList[3].Hand[3]);
            match.NextCard(match.PlayerList[0].Hand[2]);
            match.NextCard(match.PlayerList[1].Hand[1]);

            match.NextCard(match.PlayerList[3].Hand[0]);
            match.NextCard(match.PlayerList[0].Hand[3]);
            match.NextCard(match.PlayerList[1].Hand[2]);
            match.NextCard(match.PlayerList[2].Hand[1]);

            match.NextCard(match.PlayerList[0].Hand[0]);
            match.NextCard(match.PlayerList[1].Hand[3]);
            match.NextCard(match.PlayerList[2].Hand[2]);
            match.NextCard(match.PlayerList[3].Hand[1]);






            match.NextCard(match.PlayerList[1].Hand[4]);
            match.NextCard(match.PlayerList[2].Hand[7]);
            match.NextCard(match.PlayerList[3].Hand[6]);
            match.NextCard(match.PlayerList[0].Hand[5]);

            match.NextCard(match.PlayerList[2].Hand[4]);
            match.NextCard(match.PlayerList[3].Hand[7]);
            match.NextCard(match.PlayerList[0].Hand[6]);
            match.NextCard(match.PlayerList[1].Hand[5]);

            match.NextCard(match.PlayerList[3].Hand[4]);
            match.NextCard(match.PlayerList[0].Hand[7]);
            match.NextCard(match.PlayerList[1].Hand[6]);
            match.NextCard(match.PlayerList[2].Hand[5]);

            match.NextCard(match.PlayerList[0].Hand[4]);
            match.NextCard(match.PlayerList[1].Hand[7]);
            match.NextCard(match.PlayerList[2].Hand[6]);
            match.NextCard(match.PlayerList[3].Hand[5]);

            match.NextCard(match.PlayerList[1].Hand[8]);
            match.NextCard(match.PlayerList[2].Hand[11]);
            match.NextCard(match.PlayerList[3].Hand[10]);
            match.NextCard(match.PlayerList[0].Hand[9]);

            match.NextCard(match.PlayerList[2].Hand[8]);
            match.NextCard(match.PlayerList[3].Hand[11]);
            match.NextCard(match.PlayerList[0].Hand[10]);
            match.NextCard(match.PlayerList[1].Hand[9]);

            match.NextCard(match.PlayerList[3].Hand[8]);
            match.NextCard(match.PlayerList[0].Hand[11]);
            match.NextCard(match.PlayerList[1].Hand[10]);
            match.NextCard(match.PlayerList[2].Hand[9]);

            match.NextCard(match.PlayerList[0].Hand[8]);
            match.NextCard(match.PlayerList[1].Hand[11]);
            match.NextCard(match.PlayerList[2].Hand[10]);
            match.NextCard(match.PlayerList[3].Hand[9]);

            match.NextCard(match.PlayerList[1].Hand[12]);
            match.NextCard(match.PlayerList[2].Hand[12]);
            match.NextCard(match.PlayerList[3].Hand[12]);
            match.NextCard(match.PlayerList[0].Hand[12]);

            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(1))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(2))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(3))); //pas
            match.AddBid(new Contract((ContractHeight)(1), (ContractColor)(2), (PlayerTag)(0))); //1h
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(1))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(2))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(3))); //pas


            Console.WriteLine(match.CurrentBidding.CurrentPlayer.ToString());
            Console.WriteLine(match.GameState);

            match.NextCard(match.PlayerList[1].Hand[0]);
            match.NextCard(match.PlayerList[2].Hand[3]);
            match.NextCard(match.PlayerList[3].Hand[2]);
            match.NextCard(match.PlayerList[0].Hand[1]);

            match.NextCard(match.PlayerList[2].Hand[0]);
            match.NextCard(match.PlayerList[3].Hand[3]);
            match.NextCard(match.PlayerList[0].Hand[2]);
            match.NextCard(match.PlayerList[1].Hand[1]);


            match.NextCard(match.PlayerList[3].Hand[0]);
            match.NextCard(match.PlayerList[0].Hand[3]);
            match.NextCard(match.PlayerList[1].Hand[2]);
            match.NextCard(match.PlayerList[2].Hand[1]);

            match.NextCard(match.PlayerList[0].Hand[0]);
            match.NextCard(match.PlayerList[1].Hand[3]);
            match.NextCard(match.PlayerList[2].Hand[2]);
            match.NextCard(match.PlayerList[3].Hand[1]);






            match.NextCard(match.PlayerList[1].Hand[4]);
            match.NextCard(match.PlayerList[2].Hand[7]);
            match.NextCard(match.PlayerList[3].Hand[6]);
            match.NextCard(match.PlayerList[0].Hand[5]);

            match.NextCard(match.PlayerList[2].Hand[4]);
            match.NextCard(match.PlayerList[3].Hand[7]);
            match.NextCard(match.PlayerList[0].Hand[6]);
            match.NextCard(match.PlayerList[1].Hand[5]);

            match.NextCard(match.PlayerList[3].Hand[4]);
            match.NextCard(match.PlayerList[0].Hand[7]);
            match.NextCard(match.PlayerList[1].Hand[6]);
            match.NextCard(match.PlayerList[2].Hand[5]);

            match.NextCard(match.PlayerList[0].Hand[4]);
            match.NextCard(match.PlayerList[1].Hand[7]);
            match.NextCard(match.PlayerList[2].Hand[6]);
            match.NextCard(match.PlayerList[3].Hand[5]);

            match.NextCard(match.PlayerList[1].Hand[8]);
            match.NextCard(match.PlayerList[2].Hand[11]);
            match.NextCard(match.PlayerList[3].Hand[10]);
            match.NextCard(match.PlayerList[0].Hand[9]);

            match.NextCard(match.PlayerList[2].Hand[8]);
            match.NextCard(match.PlayerList[3].Hand[11]);
            match.NextCard(match.PlayerList[0].Hand[10]);
            match.NextCard(match.PlayerList[1].Hand[9]);

            match.NextCard(match.PlayerList[3].Hand[8]);
            match.NextCard(match.PlayerList[0].Hand[11]);
            match.NextCard(match.PlayerList[1].Hand[10]);
            match.NextCard(match.PlayerList[2].Hand[9]);

            match.NextCard(match.PlayerList[0].Hand[8]);
            match.NextCard(match.PlayerList[1].Hand[11]);
            match.NextCard(match.PlayerList[2].Hand[10]);
            match.NextCard(match.PlayerList[3].Hand[9]);

            match.NextCard(match.PlayerList[1].Hand[12]);
            match.NextCard(match.PlayerList[2].Hand[12]);
            match.NextCard(match.PlayerList[3].Hand[12]);
            match.NextCard(match.PlayerList[0].Hand[12]);

            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(2))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(3))); //pas
            match.AddBid(new Contract((ContractHeight)(1), (ContractColor)(2), (PlayerTag)(0))); //1h
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(1))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(2))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(3))); //pas


            Console.WriteLine(match.CurrentBidding.CurrentPlayer.ToString());
            Console.WriteLine(match.GameState);

            match.NextCard(match.PlayerList[1].Hand[0]);
            match.NextCard(match.PlayerList[2].Hand[3]);
            match.NextCard(match.PlayerList[3].Hand[2]);
            match.NextCard(match.PlayerList[0].Hand[1]);

            match.NextCard(match.PlayerList[2].Hand[0]);
            match.NextCard(match.PlayerList[3].Hand[3]);
            match.NextCard(match.PlayerList[0].Hand[2]);
            match.NextCard(match.PlayerList[1].Hand[1]);


            match.NextCard(match.PlayerList[3].Hand[0]);
            match.NextCard(match.PlayerList[0].Hand[3]);
            match.NextCard(match.PlayerList[1].Hand[2]);
            match.NextCard(match.PlayerList[2].Hand[1]);

            match.NextCard(match.PlayerList[0].Hand[0]);
            match.NextCard(match.PlayerList[1].Hand[3]);
            match.NextCard(match.PlayerList[2].Hand[2]);
            match.NextCard(match.PlayerList[3].Hand[1]);






            match.NextCard(match.PlayerList[1].Hand[4]);
            match.NextCard(match.PlayerList[2].Hand[7]);
            match.NextCard(match.PlayerList[3].Hand[6]);
            match.NextCard(match.PlayerList[0].Hand[5]);

            match.NextCard(match.PlayerList[2].Hand[4]);
            match.NextCard(match.PlayerList[3].Hand[7]);
            match.NextCard(match.PlayerList[0].Hand[6]);
            match.NextCard(match.PlayerList[1].Hand[5]);

            match.NextCard(match.PlayerList[3].Hand[4]);
            match.NextCard(match.PlayerList[0].Hand[7]);
            match.NextCard(match.PlayerList[1].Hand[6]);
            match.NextCard(match.PlayerList[2].Hand[5]);

            match.NextCard(match.PlayerList[0].Hand[4]);
            match.NextCard(match.PlayerList[1].Hand[7]);
            match.NextCard(match.PlayerList[2].Hand[6]);
            match.NextCard(match.PlayerList[3].Hand[5]);

            match.NextCard(match.PlayerList[1].Hand[8]);
            match.NextCard(match.PlayerList[2].Hand[11]);
            match.NextCard(match.PlayerList[3].Hand[10]);
            match.NextCard(match.PlayerList[0].Hand[9]);

            match.NextCard(match.PlayerList[2].Hand[8]);
            match.NextCard(match.PlayerList[3].Hand[11]);
            match.NextCard(match.PlayerList[0].Hand[10]);
            match.NextCard(match.PlayerList[1].Hand[9]);

            match.NextCard(match.PlayerList[3].Hand[8]);
            match.NextCard(match.PlayerList[0].Hand[11]);
            match.NextCard(match.PlayerList[1].Hand[10]);
            match.NextCard(match.PlayerList[2].Hand[9]);

            match.NextCard(match.PlayerList[0].Hand[8]);
            match.NextCard(match.PlayerList[1].Hand[11]);
            match.NextCard(match.PlayerList[2].Hand[10]);
            match.NextCard(match.PlayerList[3].Hand[9]);

            match.NextCard(match.PlayerList[1].Hand[12]);
            match.NextCard(match.PlayerList[2].Hand[12]);
            match.NextCard(match.PlayerList[3].Hand[12]);
            match.NextCard(match.PlayerList[0].Hand[12]);

            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(3))); //pas
            match.AddBid(new Contract((ContractHeight)(1), (ContractColor)(2), (PlayerTag)(0))); //1h
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(1))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(2))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(3))); //pas


            Console.WriteLine(match.CurrentBidding.CurrentPlayer.ToString());
            Console.WriteLine(match.GameState);

            match.NextCard(match.PlayerList[1].Hand[0]);
            match.NextCard(match.PlayerList[2].Hand[3]);
            match.NextCard(match.PlayerList[3].Hand[2]);
            match.NextCard(match.PlayerList[0].Hand[1]);

            match.NextCard(match.PlayerList[2].Hand[0]);
            match.NextCard(match.PlayerList[3].Hand[3]);
            match.NextCard(match.PlayerList[0].Hand[2]);
            match.NextCard(match.PlayerList[1].Hand[1]);


            match.NextCard(match.PlayerList[3].Hand[0]);
            match.NextCard(match.PlayerList[0].Hand[3]);
            match.NextCard(match.PlayerList[1].Hand[2]);
            match.NextCard(match.PlayerList[2].Hand[1]);

            match.NextCard(match.PlayerList[0].Hand[0]);
            match.NextCard(match.PlayerList[1].Hand[3]);
            match.NextCard(match.PlayerList[2].Hand[2]);
            match.NextCard(match.PlayerList[3].Hand[1]);






            match.NextCard(match.PlayerList[1].Hand[4]);
            match.NextCard(match.PlayerList[2].Hand[7]);
            match.NextCard(match.PlayerList[3].Hand[6]);
            match.NextCard(match.PlayerList[0].Hand[5]);

            match.NextCard(match.PlayerList[2].Hand[4]);
            match.NextCard(match.PlayerList[3].Hand[7]);
            match.NextCard(match.PlayerList[0].Hand[6]);
            match.NextCard(match.PlayerList[1].Hand[5]);

            match.NextCard(match.PlayerList[3].Hand[4]);
            match.NextCard(match.PlayerList[0].Hand[7]);
            match.NextCard(match.PlayerList[1].Hand[6]);
            match.NextCard(match.PlayerList[2].Hand[5]);

            match.NextCard(match.PlayerList[0].Hand[4]);
            match.NextCard(match.PlayerList[1].Hand[7]);
            match.NextCard(match.PlayerList[2].Hand[6]);
            match.NextCard(match.PlayerList[3].Hand[5]);

            match.NextCard(match.PlayerList[1].Hand[8]);
            match.NextCard(match.PlayerList[2].Hand[11]);
            match.NextCard(match.PlayerList[3].Hand[10]);
            match.NextCard(match.PlayerList[0].Hand[9]);

            match.NextCard(match.PlayerList[2].Hand[8]);
            match.NextCard(match.PlayerList[3].Hand[11]);
            match.NextCard(match.PlayerList[0].Hand[10]);
            match.NextCard(match.PlayerList[1].Hand[9]);

            match.NextCard(match.PlayerList[3].Hand[8]);
            match.NextCard(match.PlayerList[0].Hand[11]);
            match.NextCard(match.PlayerList[1].Hand[10]);
            match.NextCard(match.PlayerList[2].Hand[9]);

            match.NextCard(match.PlayerList[0].Hand[8]);
            match.NextCard(match.PlayerList[1].Hand[11]);
            match.NextCard(match.PlayerList[2].Hand[10]);
            match.NextCard(match.PlayerList[3].Hand[9]);

            match.NextCard(match.PlayerList[1].Hand[12]);
            match.NextCard(match.PlayerList[2].Hand[12]);
            match.NextCard(match.PlayerList[3].Hand[12]);
            match.NextCard(match.PlayerList[0].Hand[12]);

            match.AddBid(new Contract((ContractHeight)(1), (ContractColor)(2), (PlayerTag)(0))); //1h
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(1))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(2))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(3))); //pas


            Console.WriteLine(match.CurrentBidding.CurrentPlayer.ToString());
            Console.WriteLine(match.GameState);

            match.NextCard(match.PlayerList[1].Hand[0]);
            match.NextCard(match.PlayerList[2].Hand[3]);
            match.NextCard(match.PlayerList[3].Hand[2]);
            match.NextCard(match.PlayerList[0].Hand[1]);

            match.NextCard(match.PlayerList[2].Hand[0]);
            match.NextCard(match.PlayerList[3].Hand[3]);
            match.NextCard(match.PlayerList[0].Hand[2]);
            match.NextCard(match.PlayerList[1].Hand[1]);


            match.NextCard(match.PlayerList[3].Hand[0]);
            match.NextCard(match.PlayerList[0].Hand[3]);
            match.NextCard(match.PlayerList[1].Hand[2]);
            match.NextCard(match.PlayerList[2].Hand[1]);

            match.NextCard(match.PlayerList[0].Hand[0]);
            match.NextCard(match.PlayerList[1].Hand[3]);
            match.NextCard(match.PlayerList[2].Hand[2]);
            match.NextCard(match.PlayerList[3].Hand[1]);






            match.NextCard(match.PlayerList[1].Hand[4]);
            match.NextCard(match.PlayerList[2].Hand[7]);
            match.NextCard(match.PlayerList[3].Hand[6]);
            match.NextCard(match.PlayerList[0].Hand[5]);

            match.NextCard(match.PlayerList[2].Hand[4]);
            match.NextCard(match.PlayerList[3].Hand[7]);
            match.NextCard(match.PlayerList[0].Hand[6]);
            match.NextCard(match.PlayerList[1].Hand[5]);

            match.NextCard(match.PlayerList[3].Hand[4]);
            match.NextCard(match.PlayerList[0].Hand[7]);
            match.NextCard(match.PlayerList[1].Hand[6]);
            match.NextCard(match.PlayerList[2].Hand[5]);

            match.NextCard(match.PlayerList[0].Hand[4]);
            match.NextCard(match.PlayerList[1].Hand[7]);
            match.NextCard(match.PlayerList[2].Hand[6]);
            match.NextCard(match.PlayerList[3].Hand[5]);

            match.NextCard(match.PlayerList[1].Hand[8]);
            match.NextCard(match.PlayerList[2].Hand[11]);
            match.NextCard(match.PlayerList[3].Hand[10]);
            match.NextCard(match.PlayerList[0].Hand[9]);

            match.NextCard(match.PlayerList[2].Hand[8]);
            match.NextCard(match.PlayerList[3].Hand[11]);
            match.NextCard(match.PlayerList[0].Hand[10]);
            match.NextCard(match.PlayerList[1].Hand[9]);

            match.NextCard(match.PlayerList[3].Hand[8]);
            match.NextCard(match.PlayerList[0].Hand[11]);
            match.NextCard(match.PlayerList[1].Hand[10]);
            match.NextCard(match.PlayerList[2].Hand[9]);

            match.NextCard(match.PlayerList[0].Hand[8]);
            match.NextCard(match.PlayerList[1].Hand[11]);
            match.NextCard(match.PlayerList[2].Hand[10]);
            match.NextCard(match.PlayerList[3].Hand[9]);

            match.NextCard(match.PlayerList[1].Hand[12]);
            match.NextCard(match.PlayerList[2].Hand[12]);
            match.NextCard(match.PlayerList[3].Hand[12]);
            match.NextCard(match.PlayerList[0].Hand[12]);

            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(1))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(2))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(3))); //pas
            match.AddBid(new Contract((ContractHeight)(1), (ContractColor)(2), (PlayerTag)(0))); //1h
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(1))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(2))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(3))); //pas


            Console.WriteLine(match.CurrentBidding.CurrentPlayer.ToString());
            Console.WriteLine(match.GameState);

            match.NextCard(match.PlayerList[1].Hand[0]);
            match.NextCard(match.PlayerList[2].Hand[3]);
            match.NextCard(match.PlayerList[3].Hand[2]);
            match.NextCard(match.PlayerList[0].Hand[1]);

            match.NextCard(match.PlayerList[2].Hand[0]);
            match.NextCard(match.PlayerList[3].Hand[3]);
            match.NextCard(match.PlayerList[0].Hand[2]);
            match.NextCard(match.PlayerList[1].Hand[1]);


            match.NextCard(match.PlayerList[3].Hand[0]);
            match.NextCard(match.PlayerList[0].Hand[3]);
            match.NextCard(match.PlayerList[1].Hand[2]);
            match.NextCard(match.PlayerList[2].Hand[1]);

            match.NextCard(match.PlayerList[0].Hand[0]);
            match.NextCard(match.PlayerList[1].Hand[3]);
            match.NextCard(match.PlayerList[2].Hand[2]);
            match.NextCard(match.PlayerList[3].Hand[1]);






            match.NextCard(match.PlayerList[1].Hand[4]);
            match.NextCard(match.PlayerList[2].Hand[7]);
            match.NextCard(match.PlayerList[3].Hand[6]);
            match.NextCard(match.PlayerList[0].Hand[5]);

            match.NextCard(match.PlayerList[2].Hand[4]);
            match.NextCard(match.PlayerList[3].Hand[7]);
            match.NextCard(match.PlayerList[0].Hand[6]);
            match.NextCard(match.PlayerList[1].Hand[5]);

            match.NextCard(match.PlayerList[3].Hand[4]);
            match.NextCard(match.PlayerList[0].Hand[7]);
            match.NextCard(match.PlayerList[1].Hand[6]);
            match.NextCard(match.PlayerList[2].Hand[5]);

            match.NextCard(match.PlayerList[0].Hand[4]);
            match.NextCard(match.PlayerList[1].Hand[7]);
            match.NextCard(match.PlayerList[2].Hand[6]);
            match.NextCard(match.PlayerList[3].Hand[5]);

            match.NextCard(match.PlayerList[1].Hand[8]);
            match.NextCard(match.PlayerList[2].Hand[11]);
            match.NextCard(match.PlayerList[3].Hand[10]);
            match.NextCard(match.PlayerList[0].Hand[9]);

            match.NextCard(match.PlayerList[2].Hand[8]);
            match.NextCard(match.PlayerList[3].Hand[11]);
            match.NextCard(match.PlayerList[0].Hand[10]);
            match.NextCard(match.PlayerList[1].Hand[9]);

            match.NextCard(match.PlayerList[3].Hand[8]);
            match.NextCard(match.PlayerList[0].Hand[11]);
            match.NextCard(match.PlayerList[1].Hand[10]);
            match.NextCard(match.PlayerList[2].Hand[9]);

            match.NextCard(match.PlayerList[0].Hand[8]);
            match.NextCard(match.PlayerList[1].Hand[11]);
            match.NextCard(match.PlayerList[2].Hand[10]);
            match.NextCard(match.PlayerList[3].Hand[9]);

            match.NextCard(match.PlayerList[1].Hand[12]);
            match.NextCard(match.PlayerList[2].Hand[12]);
            match.NextCard(match.PlayerList[3].Hand[12]);
            match.NextCard(match.PlayerList[0].Hand[12]);

            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(2))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(3))); //pas
            match.AddBid(new Contract((ContractHeight)(1), (ContractColor)(2), (PlayerTag)(0))); //1h
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(1))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(2))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(3))); //pas


            Console.WriteLine(match.CurrentBidding.CurrentPlayer.ToString());
            Console.WriteLine(match.GameState);

            match.NextCard(match.PlayerList[1].Hand[0]);
            match.NextCard(match.PlayerList[2].Hand[3]);
            match.NextCard(match.PlayerList[3].Hand[2]);
            match.NextCard(match.PlayerList[0].Hand[1]);

            match.NextCard(match.PlayerList[2].Hand[0]);
            match.NextCard(match.PlayerList[3].Hand[3]);
            match.NextCard(match.PlayerList[0].Hand[2]);
            match.NextCard(match.PlayerList[1].Hand[1]);



            match.NextCard(match.PlayerList[3].Hand[0]);
            match.NextCard(match.PlayerList[0].Hand[3]);
            match.NextCard(match.PlayerList[1].Hand[2]);
            match.NextCard(match.PlayerList[2].Hand[1]);

            match.NextCard(match.PlayerList[0].Hand[0]);
            match.NextCard(match.PlayerList[1].Hand[3]);
            match.NextCard(match.PlayerList[2].Hand[2]);
            match.NextCard(match.PlayerList[3].Hand[1]);






            match.NextCard(match.PlayerList[1].Hand[4]);
            match.NextCard(match.PlayerList[2].Hand[7]);
            match.NextCard(match.PlayerList[3].Hand[6]);
            match.NextCard(match.PlayerList[0].Hand[5]);

            match.NextCard(match.PlayerList[2].Hand[4]);
            match.NextCard(match.PlayerList[3].Hand[7]);
            match.NextCard(match.PlayerList[0].Hand[6]);
            match.NextCard(match.PlayerList[1].Hand[5]);

            match.NextCard(match.PlayerList[3].Hand[4]);
            match.NextCard(match.PlayerList[0].Hand[7]);
            match.NextCard(match.PlayerList[1].Hand[6]);
            match.NextCard(match.PlayerList[2].Hand[5]);

            match.NextCard(match.PlayerList[0].Hand[4]);
            match.NextCard(match.PlayerList[1].Hand[7]);
            match.NextCard(match.PlayerList[2].Hand[6]);
            match.NextCard(match.PlayerList[3].Hand[5]);

            match.NextCard(match.PlayerList[1].Hand[8]);
            match.NextCard(match.PlayerList[2].Hand[11]);
            match.NextCard(match.PlayerList[3].Hand[10]);
            match.NextCard(match.PlayerList[0].Hand[9]);

            match.NextCard(match.PlayerList[2].Hand[8]);
            match.NextCard(match.PlayerList[3].Hand[11]);
            match.NextCard(match.PlayerList[0].Hand[10]);
            match.NextCard(match.PlayerList[1].Hand[9]);

            match.NextCard(match.PlayerList[3].Hand[8]);
            match.NextCard(match.PlayerList[0].Hand[11]);
            match.NextCard(match.PlayerList[1].Hand[10]);
            match.NextCard(match.PlayerList[2].Hand[9]);

            match.NextCard(match.PlayerList[0].Hand[8]);
            match.NextCard(match.PlayerList[1].Hand[11]);
            match.NextCard(match.PlayerList[2].Hand[10]);
            match.NextCard(match.PlayerList[3].Hand[9]);

            match.NextCard(match.PlayerList[1].Hand[12]);
            match.NextCard(match.PlayerList[2].Hand[12]);
            match.NextCard(match.PlayerList[3].Hand[12]);
            match.NextCard(match.PlayerList[0].Hand[12]);

            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(3))); //pas
            match.AddBid(new Contract((ContractHeight)(1), (ContractColor)(2), (PlayerTag)(0))); //1h
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(1))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(2))); //pas
            match.AddBid(new Contract((ContractHeight)(-1), (ContractColor)(-1), (PlayerTag)(3))); //pas


            Console.WriteLine(match.CurrentBidding.CurrentPlayer.ToString());
            Console.WriteLine(match.GameState);

            match.NextCard(match.PlayerList[1].Hand[0]);
            match.NextCard(match.PlayerList[2].Hand[3]);
            match.NextCard(match.PlayerList[3].Hand[2]);
            match.NextCard(match.PlayerList[0].Hand[1]);

            match.NextCard(match.PlayerList[2].Hand[0]);
            match.NextCard(match.PlayerList[3].Hand[3]);
            match.NextCard(match.PlayerList[0].Hand[2]);
            match.NextCard(match.PlayerList[1].Hand[1]);


            match.NextCard(match.PlayerList[3].Hand[0]);
            match.NextCard(match.PlayerList[0].Hand[3]);
            match.NextCard(match.PlayerList[1].Hand[2]);
            match.NextCard(match.PlayerList[2].Hand[1]);

            match.NextCard(match.PlayerList[0].Hand[0]);
            match.NextCard(match.PlayerList[1].Hand[3]);
            match.NextCard(match.PlayerList[2].Hand[2]);
            match.NextCard(match.PlayerList[3].Hand[1]);






            match.NextCard(match.PlayerList[1].Hand[4]);
            match.NextCard(match.PlayerList[2].Hand[7]);
            match.NextCard(match.PlayerList[3].Hand[6]);
            match.NextCard(match.PlayerList[0].Hand[5]);

            match.NextCard(match.PlayerList[2].Hand[4]);
            match.NextCard(match.PlayerList[3].Hand[7]);
            match.NextCard(match.PlayerList[0].Hand[6]);
            match.NextCard(match.PlayerList[1].Hand[5]);

            match.NextCard(match.PlayerList[3].Hand[4]);
            match.NextCard(match.PlayerList[0].Hand[7]);
            match.NextCard(match.PlayerList[1].Hand[6]);
            match.NextCard(match.PlayerList[2].Hand[5]);

            match.NextCard(match.PlayerList[0].Hand[4]);
            match.NextCard(match.PlayerList[1].Hand[7]);
            match.NextCard(match.PlayerList[2].Hand[6]);
            match.NextCard(match.PlayerList[3].Hand[5]);

            match.NextCard(match.PlayerList[1].Hand[8]);
            match.NextCard(match.PlayerList[2].Hand[11]);
            match.NextCard(match.PlayerList[3].Hand[10]);
            match.NextCard(match.PlayerList[0].Hand[9]);

            match.NextCard(match.PlayerList[2].Hand[8]);
            match.NextCard(match.PlayerList[3].Hand[11]);
            match.NextCard(match.PlayerList[0].Hand[10]);
            match.NextCard(match.PlayerList[1].Hand[9]);

            match.NextCard(match.PlayerList[3].Hand[8]);
            match.NextCard(match.PlayerList[0].Hand[11]);
            match.NextCard(match.PlayerList[1].Hand[10]);
            match.NextCard(match.PlayerList[2].Hand[9]);

            match.NextCard(match.PlayerList[0].Hand[8]);
            match.NextCard(match.PlayerList[1].Hand[11]);
            match.NextCard(match.PlayerList[2].Hand[10]);
            match.NextCard(match.PlayerList[3].Hand[9]);

            match.NextCard(match.PlayerList[1].Hand[12]);
            match.NextCard(match.PlayerList[2].Hand[12]);
            match.NextCard(match.PlayerList[3].Hand[12]);
            match.NextCard(match.PlayerList[0].Hand[12]);


            Console.WriteLine(match.GameState);
            Console.WriteLine(match.GameState);
            Console.WriteLine(match.GameState);

        }
    }
}
