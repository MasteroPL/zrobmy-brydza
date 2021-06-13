using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EasyHosting.Models.Client;
using GameManagerLib.Models;


namespace ClientSocketTesting
{
    public class AI
    {
        public Hand AI_hand;
        public Hand Grandpa_hand;
        public PlayedCards CardsHistory;
        public int Points;
        public int Colors5;
        public bool pass = false;
        public int[] ColorPriorityList = new int[4];

        public string Username;
        public PlayerTag Position;
        public ClientSocket ClientSocket = null;

        public AI(List<int> C, List<int> D, List<int> H, List<int> S)
        {
            this.AI_hand = new Hand(C, D, H, S);
            this.CardsHistory = new AI.PlayedCards();
            this.Points = Count_Points();
            this.Find_Color();
            this.Grandpa_hand = null;
        }

        public void Find_Color()
        {
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
            if (bid > 45)
            {
                return 0;
            }
            return bid;
        }
        public int AI_Bid(List<int> history)
        {
            int history_length = history.Count;
            int round = history_length / 4 + 1;
            int bid = 0;
            int highest_bid = 0;

            int i = 1;
            while (highest_bid <= 10)
            {

                if (history_length < i)
                {
                    break;
                }
                highest_bid = history[history_length - i];
                i++;
            }


            if (history_length < 4)
            {

                if ((history_length >= 2 && history[history_length - 2] == 0) | history_length < 2)
                {
                    if (this.Points >= 20)
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
                            if (bid == 10)
                            {
                                bid = 11;
                            }
                            return bid;
                        }

                        else
                        {
                            bid = bid + 10;
                            if (bid == 20 && highest_bid < 20)
                            {
                                return 15;
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
                if (this.pass)
                {
                    return 0;
                }
                if (history[history_length - 4] == 0) //mamy mniej niż 12
                {
                    if (history[history_length - 2] == 0) //pas partnera
                    {
                        return 0;
                    }

                    if (history_length >= 6)
                    {
                        //mamy ponad 11 punktów gadamy z partnerem
                        //TODO
                        if (history_length >= 6)
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

                if (first)
                {
                    int last_my = history[history_length - 4];
                    int last_p = history[history_length - 2];

                    if (last_my % 10 == last_p % 10) // ten sam kolor 
                    {
                        if (last_my % 10 == 3 || last_my % 10 == 4)
                        {

                            if (this.Points >= 16)
                            {
                                bid = last_p + 20; //czwórka
                            }
                            else if (last_p < 30)
                            {
                                return 0;
                            }
                            else if (history[history_length - 3] >= last_p - 10)
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
                                if (this.Points > 16)
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
                    else // oba licytujo inne kolory
                    {
                        if (last_p == 12)
                        {
                            bid = 15;
                        }
                        else if (last_p % 10 == 3)
                        {
                            if (this.AI_hand.H.Count >= 3)
                            {
                                if (this.Points > 16)
                                {
                                    bid = 43;
                                }
                                else if (last_p > last_my - 10 & history[history_length - 3] - 10 < last_p)
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
                                if (this.Points > 16)
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
                            if (this.Points > 16)
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
                            if (this.Points > 16)
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

                if (bid > highest_bid)
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
                    if (partner_bid == 11 & Colors5 == 2)
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

        public void SetColorPriorityList(int atu, bool defense)
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
                    ColorPoints[0] = ColorPoints[0] + this.AI_hand.C[i] - 10;
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

            if (atu == 0)
            {
                ColorPoints[0] = ColorPoints[0] + this.AI_hand.C.Count;
                ColorPoints[1] = ColorPoints[1] + this.AI_hand.D.Count;
                ColorPoints[2] = ColorPoints[2] + this.AI_hand.H.Count;
                ColorPoints[3] = ColorPoints[3] + this.AI_hand.S.Count;
            }
            else
            {
                ColorPoints[0] = ColorPoints[0] - this.AI_hand.C.Count;
                ColorPoints[1] = ColorPoints[1] - this.AI_hand.D.Count;
                ColorPoints[2] = ColorPoints[2] - this.AI_hand.H.Count;
                ColorPoints[3] = ColorPoints[3] - this.AI_hand.S.Count;
            }

            if (defense == false & atu != 5)
            {
                ColorPoints[atu - 1] = 10000000;
            }
            for (int i = 0; i < ColorPoints.Length; i++)
            {
                for (int j = 0; j < ColorPoints.Length; j++)
                {
                    if (ColorPoints[j] == ColorPoints.Max())
                    {
                        ColorPriorityList[i] = j + 1;
                        ColorPoints[j] = -10000;
                        break;
                    }
                }
            }

        }

        public int PutCard(List<int> trick, int atu, Hand AI_Hand)
        {   
            if(atu == 5)
            {
                atu = 0;
            }

            if (trick.Count == 0)
            {
                int index = 0;
                List<int> cp = getList(AI_Hand, ColorPriorityList[index]);
                while (cp.Count == 0)
                {
                    index++;
                    cp = getList(AI_Hand, ColorPriorityList[index]);
                }
                int color = ColorPriorityList[index];
                List<int> cards = getList(AI_Hand, ColorPriorityList[index]);
                int highestCard = this.CardsHistory.HighestCard(ColorPriorityList[index]);
                if (cards.Contains(highestCard))
                {
                    int card = FindHighest(cards, color);
                    AI_Hand.RemoveCard(card);
                    return card;
                }
                else
                {
                    int card = FindLowest(cards, color);
                    AI_Hand.RemoveCard(card);
                    return card;
                }

            }
            if (trick.Count == 1)
            {
                int color = trick[0] % 10;
                List<int> cards = getList(AI_Hand, color);
                int highestCard = this.CardsHistory.HighestCard(color);
                int card;
                if (cards.Contains(highestCard))
                {
                    card = FindHighest(cards, color);
                    AI_Hand.RemoveCard(card);
                    return card;
                }

                if (cards.Count == 0)
                {
                    if (atu == 0 | getList(AI_Hand, atu).Count == 0)
                    {
                        return AI_Hand.DropAndRemoveCard(atu);
                    }
                    else
                    {
                        List<int> atuCards = getList(AI_Hand, atu);
                        int atuCard = FindLowest(atuCards, atu);
                        AI_Hand.RemoveCard(atuCard);
                        return atuCard;
                    }
                }
                card = FindLowest(cards, color);
                AI_Hand.RemoveCard(card);
                return card;
            }
            if (trick.Count == 2)
            {
                int color = trick[0] % 10;
                List<int> cards = getList(AI_Hand, color);
                int highestCard = this.CardsHistory.HighestCard(color);

                if (cards.Count == 0)
                {
                    if (atu == 0 | getList(AI_Hand, atu).Count == 0 | trick[0] / 10 == highestCard)
                    {
                        return AI_Hand.DropAndRemoveCard(atu);
                    }
                    else
                    {
                        List<int> atuCards = getList(AI_Hand, atu);
                        int atuCard = FindLowest(atuCards, atu);
                        if (trick[1] % 10 != atu | trick[1] < atuCard)
                        {
                            AI_Hand.RemoveCard(atuCard);
                            return atuCard;
                        }
                        else
                        {
                            atuCard = FindHigherThan(trick[1], atuCards, atu);
                            if (atuCard == -1)
                            {
                                return AI_Hand.DropAndRemoveCard(atu);
                            }
                            else
                            {
                                AI_Hand.RemoveCard(atuCard);
                                return atuCard;
                            }
                        }
                    }
                }

                int card = FindHighest(cards, color);
                if (card - 30 >= trick[0] & trick[1] % 10 != atu & card > trick[1]) 
                {
                    AI_Hand.RemoveCard(card);
                    return card;
                }
                else
                {
                    card = FindLowest(cards, color);
                    AI_Hand.RemoveCard(card);
                    return card;

                }

            }
            if (trick.Count == 3)
            {
                int color = trick[0] % 10;
                List<int> cards = getList(AI_Hand, color);
                int winner = CurrentWinner(trick, atu);
                if (winner == 1)
                {
                    if (cards.Count == 0)
                    {
                        return AI_Hand.DropAndRemoveCard(atu);
                    }
                    else
                    {
                        int card = this.FindLowest(cards, color);
                        AI_Hand.RemoveCard(card);
                        return card;
                    }

                }

                if (cards.Count == 0)
                {
                    if (atu == 0 | getList(AI_Hand, atu).Count == 0)
                    {
                        return AI_Hand.DropAndRemoveCard(atu);
                    }
                    else
                    {
                        if (trick[winner] % 10 == atu)
                        {
                            List<int>  atuCards = getList(AI_Hand, atu)
                            int atuCard = FindHigherThan(trick[1], atuCards, atu);
                            if (atuCard == -1)
                            {
                                return AI_Hand.DropAndRemoveCard(atu);
                            }
                            else
                            {
                                AI_Hand.RemoveCard(atuCard);
                                return atuCard;
                            }

                        }
                        else
                        {
                            List<int> atuCards = getList(AI_Hand, atu);
                            int atuCard = FindLowest(atuCards, atu);
                            AI_Hand.RemoveCard(atuCard);
                            return atuCard;
                        }
                    }
                }
                else
                {
                    if (trick[winner] % 10 == atu)
                    {
                        int card = FindLowest(cards, color);
                        AI_Hand.RemoveCard(card);
                        return card;
                    }
                    else
                    {
                        int card = FindHigherThan(trick[1], cards, color);
                        if (card == -1)
                        {
                            card = FindLowest(cards, color);
                            AI_Hand.RemoveCard(card);
                            return card;
                        }
                        else
                        {
                            AI_Hand.RemoveCard(card);
                            return card;
                        }
                    }
                }
            }
            return 0;
        }
        // zwraca niskie
        public List<int> getList(Hand hand, int color)
        {
            if(color == 0 | color == 5)
            {
                List<int> empty = new List<int>();
                return empty;
            }
            if (color == 1)
            {
                return hand.C;
            }
            if (color == 2)
            {
                return hand.D;
            }
            if (color == 3)
            {
                return hand.H;
            }
            if (color == 4)
            {
                return hand.S;
            }

            return null;
        }

        public int FindLowest(List<int> cards, int color)
        {
            int min = cards[0];
            for (int i = 1; i < cards.Count; i++)
            {
                if (cards[i] < min)
                {
                    min = cards[i];
                }
            }
            return min * 10 + color;
        }

        public int FindHighest(List<int> cards, int color)
        {
            int max = cards[0];
            for (int i = 1; i < cards.Count; i++)
            {
                if (cards[i] > max)
                {
                    max = cards[i];
                }
            }
            return max * 10 + color;
        }
        public int FindHigherThan(int hisCard, List<int> cards, int color)
        {
            int max = FindHighest(cards, color) / 10;

            if (max < hisCard)
            {
                return -1;
            }
            else
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    if (cards[i] < max & cards[i] > hisCard)
                    {
                        max = cards[i];
                    }
                }
                return max * 10 + color;
            }
        }

        public int CurrentWinner(List<int> trick, int atu)
        {
            int max = trick[0];
            int winner = 0;
            for (int i = 1; i < 3; i++)
            {
                if (trick[i] % 10 == atu & max % 10 != atu)
                {
                    winner = i;
                    max = trick[i];
                }
                if (max % 10 == atu & trick[i] == atu & trick[i] > max)
                {
                    winner = i;
                    max = trick[i];
                }
                if (max % 10 == trick[0] % 10 & trick[i] % 10 == trick[0] % 10 & trick[i] > max)
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
                    this.C.Remove((card - 1) / 10);
                }
                if (card % 10 == 2)
                {
                    this.D.Remove((card - 2) / 10);
                }
                if (card % 10 == 3)
                {
                    this.H.Remove((card - 3) / 10);
                }
                if (card % 10 == 4)
                {
                    this.S.Remove((card - 4) / 10);
                }
            }

            public int DropAndRemoveCard(int atu)
            {
                if (atu == 4)
                {
                    if (this.C.Count == 0 & this.D.Count == 0 & this.H.Count == 0)
                    {
                        int min = FindLowest(S);
                        this.RemoveCard(min * 10 + 4);
                        return min * 10 + 4;
                    }
                }
                if (atu == 3)
                {
                    if (this.C.Count == 0 & this.D.Count == 0 & this.S.Count == 0)
                    {
                        int min = FindLowest(H);
                        this.RemoveCard(min * 10 + 3);
                        return min * 10 + 3;
                    }
                }
                if (atu == 2)
                {
                    if (this.C.Count == 0 & this.S.Count == 0 & this.H.Count == 0)
                    {
                        int min = FindLowest(D);
                        this.RemoveCard(min * 10 + 2);
                        return min * 10 + 2;
                    }
                }
                if (atu == 1)
                {
                    if (this.S.Count == 0 & this.D.Count == 0 & this.H.Count == 0)
                    {
                        int min = FindLowest(C);
                        this.RemoveCard(min * 10 + 1);
                        return min * 10 + 1;
                    }
                }
                if ((C.Count >= D.Count | atu == 2) & (C.Count >= H.Count | atu == 3) & (C.Count >= S.Count | atu == 4))
                {
                    int min = FindLowest(C);
                    this.RemoveCard(min * 10 + 1);
                    return min * 10 + 1;
                }

                if ((D.Count > C.Count | atu == 1) & (D.Count >= H.Count | atu == 3) & (D.Count >= S.Count | atu == 4))
                {
                    int min = FindLowest(D);
                    this.RemoveCard(min * 10 + 2);
                    return min * 10 + 2;
                }

                if ((H.Count > C.Count | atu == 1) & (H.Count > D.Count | atu == 2) & (H.Count >= S.Count | atu == 4))
                {
                    int min = FindLowest(H);
                    this.RemoveCard(min * 10 + 3);
                    return min * 10 + 3;
                }

                if ((S.Count > C.Count | atu == 1) & (S.Count > D.Count | atu == 2) & (S.Count > H.Count | atu == 3))
                {
                    int min = FindLowest(S);
                    this.RemoveCard(min * 10 + 4);
                    return min * 10 + 4;
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
        public class PlayedCards
        {
            public List<List<int>> Cards;
            public List<int> C;
            public List<int> D;
            public List<int> H;
            public List<int> S;
            public PlayedCards()
            {
                this.C = new List<int>();
                this.D = new List<int>();
                this.H = new List<int>();
                this.S = new List<int>();
                Cards = new List<List<int>>();
                Cards.Append(this.C);
                Cards.Append(this.D);
                Cards.Append(this.H);
                Cards.Append(this.S);
            }

            public void AddTrick(List<int> cards)
            {
                int color;
                int figure;
                foreach(var card in cards)
                {
                    color = card % 10;
                    figure = card / 10;
                    if(color == 1)
                    {
                        C.Add(figure);
                    }
                    if (color == 2)
                    {
                        D.Add(figure);
                    }
                    if (color == 3)
                    {
                        H.Add(figure);
                    }
                    if (color == 4)
                    {
                        S.Add(figure);
                    }
                }

                Cards = new List<List<int>>();
                Cards.Append(this.C);
                Cards.Append(this.D);
                Cards.Append(this.H);
                Cards.Append(this.S);
            }

            public int HighestCard(int color)
            {
                int i = 14;
                if (Cards.Count == 0)
                {
                    return i;
                }

                while (Cards[color - 1].Contains(i))
                {
                    i--;
                }

                return i;
            }
        }
    }
}
