using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaoTheGame.Model
{
    class CardSet : Deck
    {
        private Dictionary<int, Card> _cardList = new Dictionary<int, Card>();
        public int CountCard { get => _cardList.Count; }
        public List<Card> CardList { get => _cardList.Values.ToList(); }

        public CardSet() : base()
        {

        }

        public List<string> GetCardNames()
        {
            List<string> cardNames = new List<string>();
            IEnumerable<KeyValuePair<int, Card>> cardList = _cardList;
            foreach (KeyValuePair<int, Card> card in cardList)
                cardNames.Add(card.ToString());
            return cardNames;
        }

        public void AddCard(Card cardToAdd)
        {
            _cardList.Add(cardToAdd.CardIndex, cardToAdd);
        }
        public void AddCardList(List<Card> cardList)
        {
            foreach (Card card in cardList)
            {
                _cardList.Add(card.CardIndex, card);
            }
        }

        public void Sort(SortCriteria criterium)
        {
            List<Card> newList = _cardList.Values.ToList();
            newList.Sort(new CardComparer(criterium));
            Dictionary<int, Card> newDictionary = newList.ToDictionary(x => x.CardIndex);
            _cardList = newDictionary;
        }
        public void ResetCardSet()
        {
            _cardList = new Dictionary<int, Card>();
            base.ResetCards();
        }
        public bool Deal(Card cardToDeal)
        {
            if(_cardList.Values.Contains(cardToDeal))
            {
                _cardsOnTheTableList.Add(cardToDeal.CardIndex, cardToDeal);
                _cardList.Remove(cardToDeal.CardIndex);
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
