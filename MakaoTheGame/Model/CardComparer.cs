using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaoTheGame.Model
{
    class CardComparer : IComparer<Card>
    {
        private SortCriteria _sortCardBy { get; set; }
        public CardComparer(SortCriteria sortCardBy)
        {
            _sortCardBy = sortCardBy;
        }

        public void ChangeSortCriteria(SortCriteria sortCardBy)
        {
            _sortCardBy = sortCardBy;
        }

        public int Compare(Card xCard, Card yCard)
        {
            switch (_sortCardBy)
            {
                case SortCriteria.AscendSuitsThenValues:
                    if (xCard.Suit > yCard.Suit)
                        return 1;
                    else if (xCard.Suit < yCard.Suit)
                        return -1;
                    else
                    {
                        if ((int)xCard.Value > (int)yCard.Value)
                            return 1;
                        else if ((int)xCard.Value < (int)yCard.Value)
                            return -1;
                        else
                            return 0;
                    }

                case SortCriteria.AscendValuesThenSuits:
                    if ((int)xCard.Value > (int)yCard.Value)
                        return 1;
                    else if ((int)xCard.Value < (int)yCard.Value)
                        return -1;
                    else
                    {
                        if (xCard.Suit > yCard.Suit)
                            return 1;
                        else if (xCard.Suit < yCard.Suit)
                            return -1;
                        else
                            return 0;
                    }
                case SortCriteria.DescendSuitsThenValues:
                    if (xCard.Suit < yCard.Suit)
                        return 1;
                    else if (xCard.Suit > yCard.Suit)
                        return -1;
                    else
                    {
                        if ((int)xCard.Value < (int)yCard.Value)
                            return 1;
                        else if ((int)xCard.Value > (int)yCard.Value)
                            return -1;
                        else
                            return 0;
                    }
                case SortCriteria.DescendValuesThenSuits:
                    if ((int)xCard.Value < (int)yCard.Value)
                        return 1;
                    else if ((int)xCard.Value > (int)yCard.Value)
                        return -1;
                    else
                    {
                        if (xCard.Suit < yCard.Suit)
                            return 1;
                        else if (xCard.Suit > yCard.Suit)
                            return -1;
                        else
                            return 0;
                    }

            }
            throw new Exception("Zly typ lub wartość karty");

        }
    }
}
