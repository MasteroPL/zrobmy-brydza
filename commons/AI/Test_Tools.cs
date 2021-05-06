using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI
{
    class Test_Tools
    {
        public Test_Tools()
        {

        }
        public string Translate_Bid(int bid)
        {
            int color = bid % 10;
            int n = bid / 10;

            string color_s = "";
            string n_s;

            if (color == 1)
            {
                color_s = "C";
            }
            if (color == 2)
            {
                color_s = "D";
            }
            if (color == 3)
            {
                color_s = "H";
            }
            if (color == 4)
            {
                color_s = "S";
            }
            if (color == 5)
            {
                color_s = "NT";
            }

            n_s = n.ToString();

            return n_s + color_s;
        }
    }
}
