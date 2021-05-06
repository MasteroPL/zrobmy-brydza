using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI
{
    class AI
    {
        public Hand AI_hand;
        public int Points;
        public int Colors5;


        public AI(List<int> C, List<int> D, List<int> H, List<int> S)
        {
            this.AI_hand = new Hand(C, D, H, S);
            this.Points = Count_Points();
            this.Find_Color();
        }

        public void Find_Color (){
            if (this.AI_hand.S.Count >= 5)
            {
                this.Colors5 = 4;
            }

            else if (this.AI_hand.H.Count >= 5)
            {
                this.Colors5 = 3;
            }

            else if (this.AI_hand.D.Count >= 5)
            {
                this.Colors5 = 2;
            }

            else if (this.AI_hand.C.Count >= 5)
            {
                this.Colors5 = 1;
            }

            else
            {
                this.Colors5 = 0;
            }
        }

        public int Bid(List<int> history)
        {
            int history_length = history.Count;
            int round = history_length / 4 + 1;
            int bid = 0;
            int highest_bid = 0;

            int i = 1;
            while (highest_bid <= 10)
            {
                
                if(history_length < i)
                {
                    break;
                }
                highest_bid = history[history_length - i];
                i++;
            }


            if (history_length < 4)
            {
                if((history_length >= 2 &&  history[history_length - 2] == 0) | history_length < 2)
                {
                    if (this.Points >= 18)
                    {
                        bid = 20 + this.Colors5;


                        if (bid > highest_bid)
                        {
                            if (bid == 20)
                            {
                                bid = 15;
                            }
                            return bid;
                        }

                        else
                        {
                            bid = bid + 10;
                            if (bid > highest_bid)
                            {
                                return bid;
                            }

                            return 0;
                        }

                    }

                    else if (this.Points >= 12)
                    {
                        bid = 10 + this.Colors5;


                        if (bid > highest_bid)
                        {
                            if(bid == 10)
                            {
                                bid = 11;
                            }
                            return bid;
                        }

                        else
                        {
                            bid = bid + 10;
                            if(bid == 20 && highest_bid < 20)
                            {
                                return 1;
                            }
                            if (bid > highest_bid)
                            {
                                return bid;
                            }

                            return 0;
                        }

                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    //partner zalicytował
                    int partner_bid = history[history_length - 2];
                    int partner_color = partner_bid % 10;
                    if (partner_color == 4)
                    {
                        if(this.AI_hand.S.Count >= 3)
                        {
                            bid = partner_bid + 10;
                        }
                        else
                        {
                            bid = 10 + this.Colors5;
                            if(bid == 10)
                            {
                                bid = 15;
                            }

                        }
                    }
                    else if (partner_color == 3)
                    {
                        if (this.AI_hand.H.Count >= 3)
                        {
                            bid = partner_bid + 10;
                        }
                        else
                        {
                            bid = 10 + this.Colors5;
                            if (bid == 10)
                            {
                                bid = 15;
                            }

                        }
                    }
                    else if (partner_color == 2)
                    {
                        if(this.Colors5 == 4)
                        {
                            bid = 14;
                        }
                        else if (this.Colors5 == 3)
                        {
                            bid = 13;
                        }
                        else if (this.AI_hand.D.Count >= 3)
                        {
                            bid = partner_bid + 10;
                        }
                        else if(this.AI_hand.C.Count >= 5)
                        {
                            bid = 11;
                        }
                        else
                        {
                            bid = 15;
                        }
                    }
                    else if (partner_color == 1 & partner_bid >  11) //2C
                    {
                        if (this.Colors5 == 4)
                        {
                            bid = 14;
                        }
                        else if (this.Colors5 == 3)
                        {
                            bid = 13;
                        }
                        else if (this.AI_hand.C.Count >= 3)
                        {
                            bid = partner_bid + 10;
                        }
                        else if (this.AI_hand.D.Count >= 5)
                        {
                            bid = 12;
                        }
                        else
                        {
                            bid = 15;
                        }

                    }
                    else //1c lub NT
                    {
                        if (Colors5 == 0)
                        {
                            bid = 15;
                        }
                        else
                        {
                            bid = 10 + Colors5;
                        }
                    }
                    if (this.Points >= 6)
                    {
                        while(partner_bid > bid)
                        {
                            bid = bid + 10;
                        }


                        if(this.Points >= 11)
                        {
                            bid =  bid + 10;
                        }

                        if(highest_bid >= bid)
                        {
                            if(highest_bid >= bid + 10)
                            {
                                return 0;
                            }
                            return bid + 10;
                        }
                        else
                        {
                            return bid;
                        }
                    }
                    else
                    {
                        if (highest_bid == partner_bid & partner_bid == 11 & history[history_length - 1] != 1)
                        {
                            return 12;
                        }
                        return 0;
                    }

                }
                
            }
            else //kolejne kółka
            {

            }
            return -1;
        }


        private int Count_Points()
        {
            int points = 0;
            for (int i = 0; i<this.AI_hand.C.Count; i++)
            {
                if(this.AI_hand.C[i] > 10)
                {
                    points = points + this.AI_hand.C[i] - 10;
                }
            }

            for (int i = 0; i < this.AI_hand.D.Count; i++)
            {
                if (this.AI_hand.D[i] > 10)
                {
                    points = points + this.AI_hand.D[i] - 10;
                }
            }

            for (int i = 0; i < this.AI_hand.H.Count; i++)
            {
                if (this.AI_hand.H[i] > 10)
                {
                    points = points + this.AI_hand.H[i] - 10;
                }
            }

            for (int i = 0; i < this.AI_hand.S.Count; i++)
            {
                if (this.AI_hand.S[i] > 10)
                {
                    points = points + this.AI_hand.S[i] - 10;
                }
            }

            return points;
        }

        public class Hand
        {
            public List<int> C;
            public List<int> D;
            public List<int> H;
            public List<int> S;
            public Hand(List<int> C, List<int> D, List<int> H, List<int> S)
            {
                this.C = C;
                this.D = D;
                this.H = H;
                this.S = S;
            }
        }
    }
}
