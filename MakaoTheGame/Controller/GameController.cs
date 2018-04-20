using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakaoTheGame.Model;

namespace MakaoTheGame.Controller
{
    class GameController : INotifyPropertyChanged
    {


        public IEnumerable<string> CardDeck { get => Deck.GetDeckCardNames(); }

        public GameController()
        {

        }

        public void Sort(SortCriteria criterium)
        {


        }
        public void HandOutCards(int numberOfCardsPerPlayers)
        {

        }

        public void ResetAll()
        {

        }
        public void OnAllPropertyChanged()
        {
            OnPropertyChanged("CardDeck");
            OnPropertyChanged("CardSet1");
            OnPropertyChanged("CardSet2");
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
