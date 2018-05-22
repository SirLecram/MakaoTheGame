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

        public AIPlayer(GameController gameController)
        {
            GameController = gameController;
        }
        public override string ToString()
        {
            return "Gracz komp. ";
        }
    }
}
