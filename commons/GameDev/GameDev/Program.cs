using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Models;
namespace GameDev {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");

            GameInfo GameInfo = new GameInfo((ContractColor)(0),(PlayerTag)(3));
            Player P0 = new Player((PlayerTag)(0),"Derp");
            Player P1 = new Player((PlayerTag)(1), "Herp");
            Player P2 = new Player((PlayerTag)(2), "Marcin");
            Player P3 = new Player((PlayerTag)(3), "Adrian");
            Player P4 = new Player((PlayerTag)(1), "Adrian");

            GameInfo.AddPlayer(P0);
            GameInfo.AddPlayer(P1);
            GameInfo.AddPlayer(P2);
            GameInfo.AddPlayer(P3);
            GameInfo.AddPlayer(P4);

            Card C1 = new Card((CardFigure)(0), (CardColor)(1), (PlayerTag)(0)); //N
            Card C2 = new Card((CardFigure)(1), (CardColor)(1), (PlayerTag)(1)); //E
            Card C3 = new Card((CardFigure)(2), (CardColor)(1), (PlayerTag)(2)); //S
            Card C4 = new Card((CardFigure)(3), (CardColor)(1), (PlayerTag)(3));

            Card C5 = new Card((CardFigure)(8), (CardColor)(0), (PlayerTag)(0)); //N
            Card C6 = new Card((CardFigure)(14), (CardColor)(2), (PlayerTag)(1)); //E
            Card C7 = new Card((CardFigure)(9), (CardColor)(0), (PlayerTag)(2)); //S
            Card C8 = new Card((CardFigure)(3), (CardColor)(2), (PlayerTag)(3)); //W

            GameInfo.NextCard(C1);
            GameInfo.NextCard(C2);
            GameInfo.NextCard(C3);
            GameInfo.NextCard(C4);


            GameInfo.NextCard(C8);
            GameInfo.NextCard(C5);
            GameInfo.NextCard(C6);
            GameInfo.NextCard(C7);
            
            Console.WriteLine(GameInfo.TrickList.Count());
            Console.WriteLine(GameInfo.currentTrick.CardList.Count);
            Console.WriteLine(GameInfo.TrickList[1].Winner);
        }
    }
}
