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
        /// <summary>
        /// This method return list with names of card in player hand
        /// </summary>
        /// <returns></returns>
        public List<string> GetCardNames()
        {
            List<string> cardNames = new List<string>();
            IEnumerable<KeyValuePair<int, Card>> cardList = _cardList;
            foreach (KeyValuePair<int, Card> card in cardList)
                cardNames.Add(card.ToString());
            return cardNames;
        }
        /// <summary>
        /// This method add a new card to player hand
        /// </summary>
        /// <param name="cardToAdd"></param>
        public void AddCard(Card cardToAdd)
        {
            _cardList.Add(cardToAdd.CardIndex, cardToAdd);
        }
        /// <summary>
        /// This method add many cards to player hand
        /// </summary>
        /// <param name="cardList"></param>
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
        /// <summary>
        /// This method reset deck, cards on the table and cards in players hands
        /// </summary>
        public void ResetCardSet()
        {
            _cardList = new Dictionary<int, Card>();
            base.ResetCards();
        }
        /// <summary>
        /// This method provides the mechanism used for throwing cards to the table.
        /// </summary>
        /// <param name="cardToDeal"></param>
        /// <returns></returns>
        public bool Deal(Card cardToDeal)
        {
            _cardsOnTheTableList.Add(cardToDeal.CardIndex, cardToDeal);
            if (_cardList.Values.Contains(cardToDeal))
            {
                
                _cardList.Remove(cardToDeal.CardIndex);
                
            }
            return true;
        }
        public void RemoveCard(Card cardToRemove)
        {
            _cardList.Remove(cardToRemove.CardIndex);
        }

    }
}
