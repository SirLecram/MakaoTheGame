using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakaoTheGame.Controller;

namespace MakaoTheGame.Model
{
    class AIPlayer : Player
    {
        public GameController GameController { get; }
        private Random random = new Random();

        public AIPlayer(GameController gameController)
        {
            GameController = gameController;
        }
        public bool SelectCardAI()
        {
            Card lastCard = GameController._lastCard;
            ShuffleCards();
            if (PauseRoundCounter > 0)
            {
                return false;
            }
            if(GameController.isSpecialCardUsed)
            {
                if(CardList.ToList().Exists(x => x.Value == Values.Four))
                {
                    Card cardToSelect = CardList.ToList().Find(x => x.Value == Values.Four);
                    GameController.SelectCard(CardList.ToList().IndexOf(cardToSelect));
                    return true;
                }
                return false;
            }
            foreach (Card card in CardList.ToList())
            {
                if(GameController.CheckBasicCardConditions(card)
                    && (GameController.CheckAdvancedBattleCardConditions(card) /*&& GameController.CardsToTake == 1*/)
                    && GameController.CheckAdvancedSpecialCardConditions(card))
                {
                    GameController.SelectCard(CardList.ToList().IndexOf(card));
                }
            }
            if(SelectedCardsList.Count() == 2)
            {
                Card cardToTransfer = SelectedCardsList.Last();
                _cardSet.AddCard(cardToTransfer);
                _selectedCardList.Remove(cardToTransfer);
            }
            if (SelectedCardsList.Count() > 0)
            {
                
                return true;
            }   
            else
                return false;
        }
        public void ShuffleCards()
        {
            List<Card> cardList = new List<Card>();
            while (_cardSet.CardList.Count > 0)
            {
                Card cardToMove = _cardSet.CardList[random.Next(_cardSet.CardList.Count)];
                cardList.Add(cardToMove);
                _cardSet.RemoveCard(cardToMove);
            }
            _cardSet.AddCardList(cardList);
        }
        public override string ToString()
        {
            return "Gracz komp. ";
        }
    }
}
