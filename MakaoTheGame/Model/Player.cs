using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaoTheGame.Model
{
    /// <summary>
    /// Zrobione: Lista kard i ich rozdawanie
    /// </summary>
    class Player
    {
        private CardSet _cardSet = new CardSet();
        private List<Card> _selectedCardList = new List<Card>();
        public IEnumerable<Card> CardList{  get => _cardSet.CardList; }
        public IEnumerable<Card> SelectedCardsList { get => _selectedCardList; }
        public int CardCount { get => _cardSet.CountCard; }

        public void TakeCards(int numberOfCards)
        {
            _cardSet.AddCardList((List<Card>)Deck.GetCardsFromDeck(numberOfCards));
        }
        public void SelectCard(Card cardToSelect)
        {
            _selectedCardList.Add(cardToSelect);
        }
        public void AddCard(Card cardToAdd)
        {
            _cardSet.AddCard(cardToAdd);
        }
        public bool ThrowCards(Card cardToTransfer)
        {
            if (!_cardSet.Deal(cardToTransfer))
                return false;
            else
                return true;
        }
        public void Sort(SortCriteria sortCriteria)
        {
            _cardSet.Sort(sortCriteria);
        }

    }
}
