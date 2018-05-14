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
        private List<Player> PlayerList = new List<Player>();
        //private List<Card> _selectedCards = new List<Card>();
        public Player ActualPlayer { get => PlayerList[0]; }

        public IEnumerable<Card> CardList { get => ActualPlayer.CardList; }
        public IEnumerable<Card> SelectedCardsList { get => ActualPlayer.SelectedCardsList.ToList(); }
        private IEnumerable<string> CardDeck { get => Deck.GetDeckCardNames(); }

        public GameController()
        {
            PlayerList.Add(new Player());
        }

        public void NextRound()
        {
            ActualPlayer.TakeCards(7);
            OnPropertyChanged("CardList");
        }
        public void Sort(SortCriteria criterium)
        {
            ActualPlayer.Sort(criterium);
            OnPropertyChanged("CardList");
        }
        public void HandOutCards(int numberOfCardsPerPlayers)
        {
            Deck.HandOutCards(numberOfCardsPerPlayers, PlayerList);
        }
        public void ThrowCard(int cardIndexToThrow)
        {
            Card cardToThrow = ActualPlayer.CardList.ElementAt(cardIndexToThrow);
            ActualPlayer.ThrowCards(cardToThrow);
            OnPropertyChanged("CardList");
        }

        public void SelectCard(int selectedIndex)
        {
            Card cardToSelect = CardList.ElementAt(selectedIndex);

            ActualPlayer.SelectCard(cardToSelect);
            OnPropertyChanged("SelectedCardsList");
        }

        public void ResetAll()
        {

        }
        public void OnAllPropertyChanged()
        {
            OnPropertyChanged("CardDeck");
            OnPropertyChanged("CardList");
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
