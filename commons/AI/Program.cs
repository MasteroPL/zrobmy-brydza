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

            List<int> C1 = new List<int> { 13,12,10,2 };
            List<int> D1 = new List<int> { 14,11,7,5 };
            List<int> H1 = new List<int> { 9,2 };
            List<int> S1 = new List<int> { 8,5,3 };
            AI kakuter1 = new AI(C1,D1,H1,S1);

            List<int> C2 = new List<int> { 14,11,9,7,5};
            List<int> D2 = new List<int> { 10,6,2 };
            List<int> H2 = new List<int> {11 };
            List<int> S2 = new List<int> { 14,12,11,4 };
            AI kakuter2 = new AI(C2, D2, H2, S2);

            List<int> C3 = new List<int> { 4,3 };
            List<int> D3 = new List<int> { 8 };
            List<int> H3 = new List<int> { 13,12,10,7,6,4 };
            List<int> S3 = new List<int> { 13,10,9,6 };
            AI kakuter3 = new AI(C3, D3, H3, S3);

            List<int> C4 = new List<int> { 8,6};
            List<int> D4 = new List<int> { 13,12,9,4,3 };
            List<int> H4 = new List<int> { 14,8,5,3};
            List<int> S4 = new List<int> { 7,2 };
            AI kakuter4 = new AI(C4, D4, H4, S4);

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

        }
    }
}
