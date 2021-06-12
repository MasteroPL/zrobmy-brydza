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

            List<int> C1 = new List<int> { 14,13,12,11,10,9,8,7,6,5,4,3,2 };
            List<int> D1 = new List<int> { };
            List<int> H1 = new List<int> { };
            List<int> S1 = new List<int> {};
            AI kakuter1 = new AI(C1, D1, H1, S1);

            List<int> C2 = new List<int> { };
            List<int> D2 = new List<int> { 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2};
            List<int> H2 = new List<int> {};
            List<int> S2 = new List<int> { };
            AI kakuter2 = new AI(C2, D2, H2, S2);

            List<int> C3 = new List<int> {};
            List<int> D3 = new List<int> {};
            List<int> H3 = new List<int> { 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            List<int> S3 = new List<int> { };
            AI kakuter3 = new AI(C3, D3, H3, S3);

            List<int> C4 = new List<int> {};
            List<int> D4 = new List<int> {  };
            List<int> H4 = new List<int> { };
            List<int> S4 = new List<int> { 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            AI kakuter4 = new AI(C4, D4, H4, S4);

            List<int> trick = new List<int>();
            int atu = 4;

            kakuter1.SetColorPriorityList(atu, false);
            kakuter2.SetColorPriorityList(atu, true);
            kakuter3.SetColorPriorityList(atu, false);
            kakuter4.SetColorPriorityList(atu, true);


            trick.Add(kakuter2.PutCard(trick,atu, kakuter2.AI_hand));
            Console.WriteLine(trick[0]);
            trick.Add(kakuter3.PutCard(trick, atu, kakuter3.AI_hand));
            Console.WriteLine(trick[1]);
            trick.Add(kakuter4.PutCard(trick, atu, kakuter4.AI_hand));
            Console.WriteLine(trick[2]);
            trick.Add(kakuter1.PutCard(trick, atu, kakuter1.AI_hand));
            Console.WriteLine(trick[3]);

            trick = new List<int>();

            trick.Add(kakuter4.PutCard(trick, atu, kakuter4.AI_hand));
            Console.WriteLine(trick[0]);
            trick.Add(kakuter1.PutCard(trick, atu, kakuter1.AI_hand));
            Console.WriteLine(trick[1]);
            trick.Add(kakuter2.PutCard(trick, atu, kakuter2.AI_hand));
            Console.WriteLine(trick[2]);
            trick.Add(kakuter3.PutCard(trick, atu, kakuter3.AI_hand));
            Console.WriteLine(trick[3]);






            trick = new List<int>();

            trick.Add(kakuter4.PutCard(trick, atu, kakuter4.AI_hand));
            trick.Add(kakuter1.PutCard(trick, atu, kakuter1.AI_hand));
            trick.Add(kakuter2.PutCard(trick, atu, kakuter2.AI_hand));
            trick.Add(kakuter3.PutCard(trick, atu, kakuter3.AI_hand));

            Console.WriteLine(trick[0]);
            Console.WriteLine(trick[1]);
            Console.WriteLine(trick[2]);
            Console.WriteLine(trick[3]);

            trick = new List<int>();

            trick.Add(kakuter4.PutCard(trick, atu, kakuter4.AI_hand));
            trick.Add(kakuter1.PutCard(trick, atu, kakuter1.AI_hand));
            trick.Add(kakuter2.PutCard(trick, atu, kakuter2.AI_hand));
            trick.Add(kakuter3.PutCard(trick, atu, kakuter3.AI_hand));

            Console.WriteLine(trick[0]);
            Console.WriteLine(trick[1]);
            Console.WriteLine(trick[2]);
            Console.WriteLine(trick[3]);

            trick = new List<int>();

            trick.Add(kakuter4.PutCard(trick, atu, kakuter4.AI_hand));
            trick.Add(kakuter1.PutCard(trick, atu, kakuter1.AI_hand));
            trick.Add(kakuter2.PutCard(trick, atu, kakuter2.AI_hand));
            trick.Add(kakuter3.PutCard(trick, atu, kakuter3.AI_hand));

            Console.WriteLine(trick[0]);
            Console.WriteLine(trick[1]);
            Console.WriteLine(trick[2]);
            Console.WriteLine(trick[3]);


        }
    }
}
