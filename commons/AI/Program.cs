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
            

            List<int> C1 = new List<int> { 14, 12, 11 };
            List<int> D1 = new List<int> { 14, 10, 8 };
            List<int> H1 = new List<int> { 14, 10, 5, 4 };
            List<int> S1 = new List<int> { 13, 8, 2 };
            AI kakuter1 = new AI(C1,D1,H1,S1);

            List<int> C2 = new List<int> { 5,4,2};
            List<int> D2 = new List<int> { 9,7,6,4,2 };
            List<int> H2 = new List<int> { 9,7,6 };
            List<int> S2 = new List<int> { 14,3 };
            AI kakuter2 = new AI(C2, D2, H2, S2);

            List<int> C3 = new List<int> { 13,9,8,3 };
            List<int> D3 = new List<int> { 13,11,5 };
            List<int> H3 = new List<int> { 12,3,2 };
            List<int> S3 = new List<int> { 12,11,6 };
            AI kakuter3 = new AI(C3, D3, H3, S3);

            List<int> C4 = new List<int> { 10,7,6};
            List<int> D4 = new List<int> { 12,3 };
            List<int> H4 = new List<int> { 13,11,8};
            List<int> S4 = new List<int> { 10, 9, 7, 5, 4 };
            AI kakuter4 = new AI(C4, D4, H4, S4);

            List<int> h = new List<int>();
            h.Add(kakuter1.Bid(h));
            Console.WriteLine(h[0]);
            h.Add(kakuter2.Bid(h));
            Console.WriteLine(h[1]);
            h.Add(kakuter3.Bid(h));
            Console.WriteLine(h[2]);
            h.Add(kakuter4.Bid(h));
            Console.WriteLine(h[3]);

            List<int> C5 = new List<int> { 13, 8, 3 };
            List<int> D5 = new List<int> { 13, 11, 5 };
            List<int> H5 = new List<int> { 12, 3};
            List<int> S5 = new List<int> { 13, 12, 11, 6, 2 };
            AI kakuter5 = new AI(C5, D5, H5, S5);

            h = new List<int> { 0, 14, 15, 34, 0 };
            h.Add(kakuter5.Bid(h));
            Console.WriteLine(h[5]);

            h = new List<int> { 0, 14, 25, 34, 0 };
            h.Add(kakuter5.Bid(h));
            Console.WriteLine(h[5]);
        }
    }
}
