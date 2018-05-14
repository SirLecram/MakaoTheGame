using System;
using System.Collections.Generic;

namespace MakaoTheGame
{
    public enum Suits
    {
        Spade,
        Club,
        Diamond,
        Heart,
    }
    public static class SuitsExtensions
    {
        public static string ToCustomSymbol(this Suits me)
        {
            switch(me)
            {
                case Suits.Spade:
                    return "♠";
                case Suits.Club:
                    return "♣";
                case Suits.Diamond:
                    return "♦";
                case Suits.Heart:
                    return "♥";
            }
            return null;
        }
    }

    enum Values
    {
        Two = 2,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace,
    }
    public enum TransferOptions
    {
        P1ToDeck,
        DeckToP1,
        P2ToDeck,
        DeckToP2,
    }

    public enum SortCriteria
    {
        AscendSuitsThenValues,
        DescendSuitsThenValues,
        AscendValuesThenSuits,
        DescendValuesThenSuits,
    }
    public static class SortCriteriaExtensions
    {
        public static string ToCustomString(this SortCriteria me)
        {
            switch (me)
            {
                case SortCriteria.AscendSuitsThenValues:
                    return "      Rosnąco \nFigura>Wartość";
                case SortCriteria.DescendSuitsThenValues:
                    return "      Malejąco \nFigura>Wartość";
                case SortCriteria.AscendValuesThenSuits:
                    return "      Rosnąco \nWartość>Figura";
                case SortCriteria.DescendValuesThenSuits:
                    return "      Malejąco \nWartość>Figura";
            }
            return null;
        }
        public static List<string> GetNamesList()
        {
            List<string> list = new List<string>();
            foreach (SortCriteria criterium in Enum.GetValues(typeof(SortCriteria)))
                list.Add(ToCustomString(criterium));
            return list;
        }

    }
    public enum CardType
    {
        Free,
        War,
        Special,
    }
}