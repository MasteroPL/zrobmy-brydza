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
        public bool pass = false;
        public int[] ColorPriorityList = new int[4];

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
            int bid = AI_Bid(history);
            while(bid > 75)
            {
                bid = bid - 10;
            }
            return bid;
        }
        public int AI_Bid(List<int> history)
        {
            int history_length = history.Count;
            int round = history_length / 4 + 1;
            int bid = 0 ;
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
                    return FirstResponse(history_length, history, highest_bid);
                }
                
            }
            else //kolejne kółka
            {
                if(this.pass)
                {
                    return 0;
                }
                if (history[history_length - 4] == 0) //mamy mniej niż 12
                {
                    if (history[history_length - 2] == 0) //pas partnera
                    {
                        return 0;
                    }

                    if(history_length >= 6 )
                    {
                        //mamy ponad 11 punktów gadamy z partnerem
                        //TODO
                        if(history_length >= 6)
                        {
                            if (history[history_length - 6] != 0)
                            {
                                return 0;
                            }
                        }
                    }
                    //odpowiedź na pierwsze wyjście partnera
                    return FirstResponse(history_length, history, highest_bid);
                }
                bool first = false;
                if (history_length % 4 < 3)
                {
                    first = true;
                }
                else if (history[history_length - (round - 1) * 4 - 2] == 0)
                {
                    first = true;
                }
                   
                if(first)
                {
                    int last_my = history[history_length - 4];
                    int last_p = history[history_length - 2];

                    if (last_my % 10 == last_p % 10) // ten sam kolor 
                    {
                        if(last_my % 10 == 3 || last_my % 10 == 4)
                        {

                            if(this.Points >= 16)
                            {
                                bid = last_p + 20; //czwórka
                            }
                            else if (last_p < 30)
                            {
                                return 0;
                            }
                            else if(history[history_length - 3] >= last_p -10)
                            {
                                return 0;
                            }
                            else if (last_p > last_my - 10)
                            {
                                bid = last_p + 10; //czwórka
                            }
                            else
                            {
                                return 0;
                            }
                        }
                        if (last_my % 10 == 5)
                        {
                            if (last_p > 25)
                            {
                                return 0;
                            }
                            else
                            {
                                if(this.Points > 16)
                                {
                                    bid = 35;
                                }
                            }
                        }
                        if (last_my % 10 == 1 | last_my % 10 == 2)
                        {
                            if (this.Points > 16)
                            {
                                bid = 35;
                            }
                            else if(last_p > last_my - 10 & history[history_length - 3] -10 < last_p )
                            {
                                bid = 35;
                            }
                            else
                            {
                                return 0;
                            }
                        }
                    }
                    else // oba licytujo inne kolory
                    {
                        if(last_p == 12)
                        {
                            bid = 15;
                        }
                        else if(last_p % 10 == 3 )
                        {
                           if( this.AI_hand.H.Count >= 3)
                           {
                                if(this.Points == 16)
                                {
                                    bid = 43;
                                }
                                else if(last_p > last_my - 10 & history[history_length - 3] - 10 < last_p)
                                {
                                    bid = 43;
                                }
                                else
                                {
                                    return 0;
                                }
                           }
                        }
                        else if (last_p % 10 == 4)
                        {
                            if (this.AI_hand.S.Count >= 3)
                            {
                                if (this.Points == 16)
                                {
                                    bid = 44;
                                }
                                else if (last_p > last_my - 10 & history[history_length - 3] - 10 < last_p)
                                {
                                    bid = 44;
                                }
                                else
                                {
                                    return 0;
                                }
                            }
                        }
                        else if (last_p == 22 & last_my == 11)
                        {
                            if(this.Points > 16)
                            {
                                bid = 35;
                            }
                            else
                            {
                                bid = 15;
                            }
                        }
                        else if (last_p % 10 == 5 | last_p % 10 == 1 | last_p % 10 == 2)
                        {
                            if(this.Points > 16)
                            {
                                bid = 35;
                            }
                            else if (last_p > last_my - 10 & history[history_length - 3] - 10 < last_p)
                            {
                                bid = 35;
                            }
                            else
                            {
                                return 0;
                            }
                        }

                    }

                }

                if(bid > highest_bid)
                {
                    return bid;
                }
                else
                {
                    return 0;
                }

            }
            return -1;
        }

        public int FirstResponse(int history_length, List<int> history, int highest_bid)
        {
            int bid = 0;
            int partner_bid = history[history_length - 2];
            int partner_color = partner_bid % 10;
            if (partner_color == 4)
            {
                if (this.AI_hand.S.Count >= 3)
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
                if (this.Colors5 == 4)
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
                else if (this.AI_hand.C.Count >= 5)
                {
                    bid = 11;
                }
                else
                {
                    bid = 15;
                }
            }
            else if (partner_color == 1 & partner_bid > 11) //2C
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
                    if(partner_bid == 11 & Colors5 == 2)
                    {
                        bid = bid + 10;
                    }
                }
            }
            if (this.Points >= 6)
            {
                while (partner_bid >= bid)
                {
                    bid = bid + 10;
                }


                if (this.Points >= 11)
                {
                    bid = bid + 10;
                }

                if (highest_bid >= bid)
                {
                    if (highest_bid >= bid + 10)
                    {
                        this.pass = true;
                        return 0;
                    }
                    if (this.Points < 12)
                    {
                        this.pass = true;
                    }
                    return bid + 10;
                }
                else
                {
                    if (this.Points < 12)
                    {
                        this.pass = true;
                    }
                    return bid;
                }


            }
            else
            {
                if (highest_bid == partner_bid & partner_bid == 11 & history[history_length - 1] != 1)
                {
                    this.pass = true;
                    return 12;
                }
                return 0;
            }
        }


        private int Count_Points()
        {
            int points = 0;
            for (int i = 0; i < this.AI_hand.C.Count; i++)
            {
                if (this.AI_hand.C[i] > 10)
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
        // ROZGRYWKA

        public void SetColorPriorityList(List<int> history, int atu, bool defense)
        {
            int[] ColorPoints = new int[4];
            for (int i = 0; i < ColorPoints.Length; i++)
            {
                ColorPoints[i] = 0;
            }

            for (int i = 0; i < this.AI_hand.C.Count; i++)
            {
                if (this.AI_hand.C[i] > 10)
                {
                    ColorPoints[1] = ColorPoints[1] + this.AI_hand.C[i] - 10;
                }
            }

            for (int i = 0; i < this.AI_hand.D.Count; i++)
            {
                if (this.AI_hand.D[i] > 10)
                {
                    ColorPoints[1] = ColorPoints[1] + this.AI_hand.D[i] - 10;
                }
            }

            for (int i = 0; i < this.AI_hand.H.Count; i++)
            {
                if (this.AI_hand.H[i] > 10)
                {
                    ColorPoints[2] = ColorPoints[2] + this.AI_hand.H[i] - 10;
                }
            }

            for (int i = 0; i < this.AI_hand.S.Count; i++)
            {
                if (this.AI_hand.S[i] > 10)
                {
                    ColorPoints[3] = ColorPoints[3] + this.AI_hand.S[i] - 10;
                }
            }

            if(atu == 0 )
            {
                ColorPoints[1] = ColorPoints[1] + this.AI_hand.C.Count;
                ColorPoints[1] = ColorPoints[1] + this.AI_hand.D.Count;
                ColorPoints[2] = ColorPoints[2] + this.AI_hand.H.Count;
                ColorPoints[3] = ColorPoints[3] + this.AI_hand.S.Count;
            }
            else
            {
                ColorPoints[1] = ColorPoints[1] - this.AI_hand.C.Count;
                ColorPoints[1] = ColorPoints[1] - this.AI_hand.D.Count;
                ColorPoints[2] = ColorPoints[2] - this.AI_hand.H.Count;
                ColorPoints[3] = ColorPoints[3] - this.AI_hand.S.Count;
            }

            if (defense == false)
            {
                ColorPoints[atu] = 10000000;
            }
            for (int i = 0; i < ColorPoints.Length; i++ )
            {
                for (int j = 0; j < ColorPoints.Length; j++)
                {
                    if (ColorPoints[j] == ColorPoints.Max())
                    {
                        ColorPriorityList[i] = j;
                        ColorPoints[j] = -10000;
                    }
                }
            } 
            
        } 

        public int PutCard(List<int> trick, int atu, List<int> highestCards )
        {
            if(trick.Count == 0)
            {
                int index = 0;
                while(getList(ColorPriorityList[index]).Count == 0)
                {
                    index++;
                }
                List<int> cards = getList(ColorPriorityList[index]);
                if(cards.Max() == highestCards[ColorPriorityList[index]])
                {
                    this.AI_hand.RemoveCard(cards.Max());
                    return cards.Max();
                }
                else
                {
                    this.AI_hand.RemoveCard(cards.Min());
                    return cards.Min();
                }

            }
            if (trick.Count == 1)
            {
                int color = trick[0] % 10;
                List<int> cards = getList(color);
                int highestCard = highestCards[color - 1];
                if(cards.Contains(highestCard))
                {
                    this.AI_hand.RemoveCard(highestCard);
                    return highestCard;
                }

                if(cards.Count == 0)
                {
                    if(atu == 0 | getList(atu).Count == 0)
                    {
                        return this.AI_hand.DropAndRemoveCard();
                    }
                    else
                    {
                        List<int> atuCards = getList(atu);
                        int atuCard = FindLowest(atuCards);
                        this.AI_hand.RemoveCard(atuCard);
                        return atuCard;
                    }
                }
                int card = FindLowest(cards);
                this.AI_hand.RemoveCard(card);
                return card;
            }
            if (trick.Count == 2)
            {
                int color = trick[0] % 10;
                List<int> cards = getList(color);
                int highestCard = highestCards[color - 1];

                if (cards.Count == 0)
                {
                    if ((atu == 0 | getList(atu).Count == 0) & trick[0] != highestCard)
                    {
                        return this.AI_hand.DropAndRemoveCard();
                    }
                    else
                    {
                        List<int> atuCards = getList(atu);
                        int atuCard = FindLowest(atuCards);
                        if (trick[1]%10 != atu | trick[1] < atuCard)
                        {
                            this.AI_hand.RemoveCard(atuCard);
                            return atuCard;
                        }
                        else
                        {
                            atuCard = FindHigherThan(trick[1], cards);
                            if( atuCard == -1)
                            {
                                return this.AI_hand.DropAndRemoveCard(); // TODO czy nie atu !!!!!!!!!!!!!!!
                            }
                            else
                            {
                                this.AI_hand.RemoveCard(atuCard);
                                return atuCard;
                            }
                        }
                    }
                }
                int card = FindHighest(cards);
                if(card-30 >= trick[0] & trick[1]%10 != atu) //TODO wszytskie karty
                {
                    this.AI_hand.RemoveCard(card);
                    return card;
                }
                else
                {
                    card = FindLowest(cards);
                    this.AI_hand.RemoveCard(card);
                    return card;

                }

            }
            if (trick.Count == 3)
            {
                int color = trick[0] % 10;
                List<int> cards = getList(color);
                int winner = CurrentWinner(trick, atu);
                if(winner == 1)
                {
                    return this.AI_hand.DropAndRemoveCard(); // TODO czy nie atu !!!!!!!!!!!!!!!
                }

                if (cards.Count == 0)
                {
                    if (atu == 0 | getList(atu).Count == 0)
                    {
                        return this.AI_hand.DropAndRemoveCard();
                    }
                    else
                    {
                        if(trick[winner] % 10 == atu)
                        {
                            int atuCard = FindHigherThan(trick[1], cards);
                            if (atuCard == -1)
                            {
                                return this.AI_hand.DropAndRemoveCard(); // TODO czy nie atu !!!!!!!!!!!!!!!
                            }
                            else
                            {
                                this.AI_hand.RemoveCard(atuCard);
                                return atuCard;
                            }

                        }
                        else
                        {
                            List<int> atuCards = getList(atu);
                            int atuCard = FindLowest(atuCards);
                            this.AI_hand.RemoveCard(atuCard);
                            return atuCard;
                        }
                    }
                }
                else
                {
                    if (trick[winner] % 10 == atu)
                    {
                        int card = FindLowest(cards);
                        this.AI_hand.RemoveCard(card);
                        return card;
                    }
                    else
                    {
                        int card = FindHigherThan(trick[1], cards);
                        if (card == -1)
                        {
                            card = FindLowest(cards);
                            this.AI_hand.RemoveCard(card);
                            return card;
                        }
                        else
                        {
                            this.AI_hand.RemoveCard(card);
                            return card;
                        }
                    }
                }
            }
            return 0;
        }

        public List<int> getList(int color)
        {
            if(color == 1)
            {
                return this.AI_hand.C;
            }
            if (color == 2)
            {
                return this.AI_hand.D;
            }
            if (color == 3)
            {
                return this.AI_hand.H;
            }
            if (color == 4)
            {
                return this.AI_hand.S;
            }

            return null;
        }

        public int FindLowest( List<int>cards)
        {
            int min = cards[0];
            for (int i = 1;i < cards.Count; i++)
            {
                if(cards[i]< min)
                {
                    min = cards[i];
                }
            }
            return min;
        }

        public int FindHighest(List<int> cards)
        {
            int max = cards[0];
            for (int i = 1; i < cards.Count; i++)
            {
                if (cards[i] > max)
                {
                    max = cards[i];
                }
            }
            return max;
        }
        public int FindHigherThan(int hisCard, List<int> cards)
        {
            int max = FindHighest(cards);

            if(max < hisCard)
            {
                return -1;
            }
            else
            {   
                for(int i = 0; i < cards.Count; i++)
                {
                    if(cards[i] < max & cards[i] > hisCard)
                    {
                        max = cards[i];
                    }
                }
                return max;
            }
        }

        public int CurrentWinner(List<int> trick, int atu)
        {
            int max = trick[0];
            int winner = 0;
            for (int i = 1; i < 3;i++)
            {
                if(trick[i] % 10 == atu & max %10 != atu)
                {
                    winner = i;
                    max = trick[i];
                }
                if(max % 10 == atu & trick[i] == atu & trick[i] > max )
                {
                    winner = i;
                    max = trick[i];
                }
                if (max % 10 == trick[0] %10  & trick[i] % 10  == trick[0] % 10 & trick[i] > max )
                {
                    winner = i;
                    max = trick[i];
                }
            }
            return winner;
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
            public void RemoveCard(int card)
            {
                if (card % 10 == 1)
                {
                    this.C.Remove(card);
                }
                if (card % 10 == 2)
                {
                    this.D.Remove(card);
                }
                if (card % 10 == 3)
                {
                    this.H.Remove(card);
                }
                if (card % 10 == 4)
                {
                    this.S.Remove(card);
                }
            }

            public int DropAndRemoveCard()
            {
 
                if(C.Count >= D.Count & C.Count >= H.Count & C.Count >= S.Count)
                {
                    int min = FindLowest(C);
                    this.RemoveCard(min);
                    return min;
                }

                if (D.Count > C.Count & D.Count >= H.Count & D.Count >= S.Count)
                {
                    int min = FindLowest(D);
                    this.RemoveCard(min);
                    return min;
                }

                if (H.Count > C.Count & H.Count > D.Count & H.Count >= S.Count)
                {
                    int min = FindLowest(H);
                    this.RemoveCard(min);
                    return min;
                }

                if (S.Count > H.Count & S.Count > D.Count & S.Count > H.Count)
                {
                    int min = FindLowest(S);
                    this.RemoveCard(min);
                    return min;
                }
                return 0;
            }
            public int FindLowest(List<int> cards)
            {
                int min = cards[0];
                for (int i = 1; i < cards.Count; i++)
                {
                    if (cards[i] < min)
                    {
                        min = cards[i];
                    }
                }
                return min;
            }
        }
    }
}
