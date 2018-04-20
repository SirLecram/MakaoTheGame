using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaoTheGame.Model
{
    abstract class CardEntity
    {
        protected static int _cardCounter = 0;


        protected CardEntity()
        {
            _cardCounter++;
        }

    }
}
