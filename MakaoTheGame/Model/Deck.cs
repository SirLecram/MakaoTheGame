using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaoTheGame.Model
{
    abstract class Deck
    {
        protected static Random random = new Random();
        protected static Dictionary<int, Card> _deckCardList = new Dictionary<int, Card>();
        protected static Dictionary<int, Card> _cardsOnTheTableList = new Dictionary<int, Card>();
        public List<Card> CardDeckList { get => _deckCardList.Values.ToList(); }
        public int CardDeckListCount { get => _deckCardList.Count; }

        public Deck()
        {
            ResetCards();
        }

        protected virtual void ResetCards()
        {
            _cardsOnTheTableList.Clear();
            Card.ResetCardEntity();
            _deckCardList.Clear();
            for (int suits = 0; suits < 4; suits++)
                for (int values = 2; values < 15; values++)
                {
                    Card newCard = new Card((Suits)suits, (Values)values);
                    _deckCardList.Add(newCard.CardIndex, newCard);
                }
            
        }

        public static void ShuffleDeck() // DO PRZETESTOWANIA
        {
            Dictionary<int, Card> cardList = new Dictionary<int, Card>();
            while (_deckCardList.Count > 0)
            {
                Card cardToMove = _deckCardList.Values.ToList()[random.Next(_deckCardList.Count)];
                cardList.Add(cardToMove.CardIndex, cardToMove);
                _deckCardList.Remove(cardToMove.CardIndex);
            }
            _deckCardList = cardList;
        }
        private static void ReShuffleCardsOnTable()
        {
            Dictionary<int, Card> cardList = new Dictionary<int, Card>();
            Card lastCardOnTheTable = _cardsOnTheTableList.Values.Last();
            while (_cardsOnTheTableList.Count > 0)
            {
                Card cardToMove = _cardsOnTheTableList.Values.ToList()[random.Next(_cardsOnTheTableList.Count)];
                cardList.Add(cardToMove.CardIndex, cardToMove);
                _cardsOnTheTableList.Remove(cardToMove.CardIndex);
            }
            cardList.Remove(lastCardOnTheTable.CardIndex);
            _cardsOnTheTableList = cardList;
            _deckCardList = (_deckCardList.Concat(_cardsOnTheTableList)).ToDictionary(x => x.Key, y => y.Value);
            _cardsOnTheTableList.Clear();
            _cardsOnTheTableList.Add(lastCardOnTheTable.CardIndex, lastCardOnTheTable);
        }

        public static void HandOutCards(int numberOfCardsPerPlayer, List<Player> playerList)
        {
            ShuffleDeck(); ShuffleDeck();
            for(int i = 0; i< numberOfCardsPerPlayer; i++)
            {
                foreach(Player player in playerList)
                {
                    Card cardToTransfer = _deckCardList.Values.ElementAt(0);
                    player.AddCard(cardToTransfer);
                    _deckCardList.Remove(cardToTransfer.CardIndex);
                }
            }
        }
        // DO ZROBIENIA : Warunek, który przetasuje talie gdy za malo kart.
        public static IEnumerable<Card> GetCardsFromDeck(int numberOfCards)
        {
            List<Card> cardlist = new List<Card>();
            if(_deckCardList.Count > numberOfCards)
            {
                for (int i = 0; i < numberOfCards; i++)
                {
                    Card cardToTransfer = _deckCardList.Values.ElementAt(0);
                    cardlist.Add(cardToTransfer);
                    _deckCardList.Remove(cardToTransfer.CardIndex);
                }
            }
            else
            {
                ReShuffleCardsOnTable();
                cardlist = (List<Card>) GetCardsFromDeck(numberOfCards);
            }
            return cardlist;
           
        }
        
        public static List<string> GetDeckCardNames()
        {
            List<string> cardNames = new List<string>();
            IEnumerable<KeyValuePair<int, Card>> cardList = _deckCardList;
            foreach (KeyValuePair<int, Card> card in cardList)
                cardNames.Add(/*card.Value.ToString()*/card.ToString());
            return cardNames;
        }
        public static List<string> GetTableCardsNames()
        {
            List<string> cardNames = new List<string>();
            IEnumerable<KeyValuePair<int, Card>> cardList = _cardsOnTheTableList;
            foreach (KeyValuePair<int, Card> card in cardList)
                cardNames.Add(/*card.Value.ToString()*/card.ToString());
            return cardNames;
        }

    }
}
