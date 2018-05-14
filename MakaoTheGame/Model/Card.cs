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

        public Card(Suits suit, Values value) : base()
        {
            CardIndex = _cardCounter;
            Suit = suit;
            Value = value;
        }

        public static void ResetCardEntity()
        {
            _cardCounter = 0;
        }
        public override string ToString()
        {

            return Value.ToString() + " of " + Suit.ToString() + " " + 
                SuitsExtensions.ToCustomSymbol(Suit);
        }


    }
}
