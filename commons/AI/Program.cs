using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI
{
    
    class Program
    {
        static void Main(string[] args)
        {
            Test_Tools tools = new Test_Tools();

            List<int> C1 = new List<int> { 6,2 };
            List<int> D1 = new List<int> { 3,2 };
            List<int> H1 = new List<int> { 11,6,2 };
            List<int> S1 = new List<int> {14,12,9,8,6,5 };
            AI kakuter1 = new AI(C1, D1, H1, S1);

            List<int> C2 = new List<int> { 14,13,7,5 };
            List<int> D2 = new List<int> { 11, 5, 4 };
            List<int> H2 = new List<int> {5,4,3 };
            List<int> S2 = new List<int> { 10, 4, 2 };
            AI kakuter2 = new AI(C2, D2, H2, S2);

            List<int> C3 = new List<int> {12};
            List<int> D3 = new List<int> {13,12,9,8,7 };
            List<int> H3 = new List<int> {13,10,8,7 };
            List<int> S3 = new List<int> { 13,11,3 };
            AI kakuter3 = new AI(C3, D3, H3, S3);

            List<int> C4 = new List<int> { 11,10,9,8,4,3 };
            List<int> D4 = new List<int> { 14,10,6 };
            List<int> H4 = new List<int> { 14, 12,9 };
            List<int> S4 = new List<int> { 7 };
            AI kakuter4 = new AI(C4, D4, H4, S4);
            /*
            List<int> h = new List<int>();
            h.Add(kakuter1.Bid(h));
            Console.WriteLine(tools.Translate_Bid(h[0]));
            h.Add(kakuter2.Bid(h));
            Console.WriteLine(tools.Translate_Bid(h[1]));
            h.Add(kakuter3.Bid(h));
            Console.WriteLine(tools.Translate_Bid(h[2]));
            h.Add(kakuter4.Bid(h));
            Console.WriteLine(tools.Translate_Bid(h[3]));

            h.Add(kakuter1.Bid(h));
            Console.WriteLine(tools.Translate_Bid(h[4]));
            h.Add(kakuter2.Bid(h));
            Console.WriteLine(tools.Translate_Bid(h[5]));
            h.Add(kakuter3.Bid(h));
            Console.WriteLine(tools.Translate_Bid(h[6]));
            h.Add(kakuter4.Bid(h));
            Console.WriteLine(tools.Translate_Bid(h[7]));

            h.Add(kakuter1.Bid(h));
            Console.WriteLine(tools.Translate_Bid(h[8]));
            h.Add(kakuter2.Bid(h));
            Console.WriteLine(tools.Translate_Bid(h[9]));
            h.Add(kakuter3.Bid(h));
            Console.WriteLine(tools.Translate_Bid(h[10]));
            h.Add(kakuter4.Bid(h));
            Console.WriteLine(tools.Translate_Bid(h[11]));
            */

            //public int PutCard(List<int> trick, int atu, List<int> highestCards)
            //SetColorPriorityList(List<int> history, int atu, bool defense)
            
            List<int> trick = new List<int>();
            int atu = 4;

            kakuter1.SetColorPriorityList(null, atu, false);
            kakuter2.SetColorPriorityList(null, atu, true);
            kakuter3.SetColorPriorityList(null, atu, false);
            kakuter4.SetColorPriorityList(null, atu, true);


            trick.Add(kakuter2.PutCard(trick,atu));
            trick.Add(kakuter3.PutCard(trick, atu));
            trick.Add(kakuter4.PutCard(trick, atu));
            trick.Add(kakuter1.PutCard(trick, atu));

            Console.WriteLine(trick[0]);
            Console.WriteLine(trick[1]);
            Console.WriteLine(trick[2]);
            Console.WriteLine(trick[3]);

            trick = new List<int>();

            trick.Add(kakuter2.PutCard(trick, atu));
            trick.Add(kakuter3.PutCard(trick, atu));
            trick.Add(kakuter4.PutCard(trick, atu));
            trick.Add(kakuter1.PutCard(trick, atu));

            Console.WriteLine(trick[0]);
            Console.WriteLine(trick[1]);
            Console.WriteLine(trick[2]);
            Console.WriteLine(trick[3]);


            trick = new List<int>();

            trick.Add(kakuter3.PutCard(trick, atu));
            trick.Add(kakuter4.PutCard(trick, atu));
            trick.Add(kakuter1.PutCard(trick, atu));
            trick.Add(kakuter2.PutCard(trick, atu));

            Console.WriteLine(trick[0]);
            Console.WriteLine(trick[1]);
            Console.WriteLine(trick[2]);
            Console.WriteLine(trick[3]);


        }
    }
}
