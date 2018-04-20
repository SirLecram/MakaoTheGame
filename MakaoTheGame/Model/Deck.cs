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
        public List<Card> CardDeckList { get => _deckCardList.Values.ToList(); }

        public Deck()
        {
            ResetCards();
        }

        protected virtual void ResetCards()
        {
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

        public static void HandOutCards(int numberOfCardsPerPlayer, List<Player> playerList)
        {
           
        }
        
        public static List<string> GetDeckCardNames()
        {

            List<string> cardNames = new List<string>();
            IEnumerable<KeyValuePair<int, Card>> cardList = _deckCardList;
            foreach (KeyValuePair<int, Card> card in cardList)
                cardNames.Add(/*card.Value.ToString()*/card.ToString());
            return cardNames;
        }

    }
}
