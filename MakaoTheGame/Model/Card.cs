using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaoTheGame.Model
{
    class Card : CardEntity
    {
        public Suits Suit { get; }
        public Values Value { get; }
        public int CardIndex { get; }
        public int? BattleCard { get; }
        public bool SpecialCard { get; }

        public Card(Suits suit, Values value) : base()
        {
            CardIndex = _cardCounter;
            Suit = suit;
            Value = value;
            BattleCard = CheckIfItsBattleCard();
            SpecialCard = CheckIfItsSpecialCard();
        }

        public static void ResetCardEntity()
        {
            _cardCounter = 0;
        }
        private int? CheckIfItsBattleCard()
        {
            if (Value == (Values)2)
                return 2;
            else if (Value == (Values)3)
                return 3;
            else if (Value == Values.King && (Suit == Suits.Heart || Suit == Suits.Spade))
                return 5;
            else
                return null;
        }
        private bool CheckIfItsSpecialCard()
        {
            if (Value == Values.Jack || BattleCard.HasValue || Value == Values.Ace)
                return true;
            else
                return false;
        }
        public override string ToString()
        {

            return Value.ToString() + " of " + Suit.ToString() + " " + 
                SuitsExtensions.ToCustomSymbol(Suit);
        }


    }
}
