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
        public Values? SpecialCard { get; }

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
        private Values? CheckIfItsSpecialCard()
        {
            if (Value == Values.Jack)
                return Values.Jack;
            else if (Value == Values.Ace)
                return Values.Ace;
            else if (Value == Values.Four)
                return Values.Four;
            else
                return null;
        }
        public override string ToString()
        {

            return Value.ToString() + " of " + Suit.ToString() + " " + 
                SuitsExtensions.ToCustomSymbol(Suit);
        }


    }
}
