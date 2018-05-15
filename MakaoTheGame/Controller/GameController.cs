using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MakaoTheGame.Model;

namespace MakaoTheGame.Controller
{
    class GameController : INotifyPropertyChanged
    {
        #region Attributes
        private List<Player> PlayerList = new List<Player>();
        public Player ActualPlayer { get => PlayerList[_roundNumber - 1]; }
        private int _roundNumber = 1;
        private Card _lastCard = null;
        private IEnumerable<string> CardDeck { get => Deck.GetDeckCardNames(); }
        #endregion

        #region Attributes responsible for BINDING
        public string PlayerRound
        {
            get
            {
                if (!(ActualPlayer is AIPlayer))
                    return ActualPlayer.ToString();
                else
                    return ActualPlayer.ToString() + "nr " + (_roundNumber - 1).ToString();
            }
        }
        public IEnumerable<Card> CardList { get => ActualPlayer.CardList; }
        public IEnumerable<Card> SelectedCardsList { get => ActualPlayer.SelectedCardsList.ToList(); }
        public string GameReport { get; private set; }
        public int CardsToTake { get; private set; }
        #endregion

        public GameController()
        {
            PlayerList.Add(new Player());
            PlayerList.Add(new AIPlayer());
            PlayerList.Add(new AIPlayer());
            GameReport += "Zaczynajmy grę!\n";
            CardsToTake = 1;
            //ActualPlayer.TakeCards(7);
        }

        #region Basic game mechanics controler
        public void NextRound(bool takeCards)
        {
            if(takeCards)
            {
                ActualPlayer.TakeCards(CardsToTake);
                CardsToTake = 1;
            }
            _roundNumber++;
            if (_roundNumber > PlayerList.Count())
                _roundNumber = 1;
            OnAllPropertyChanged();

        }

        public void ResetAll()
        {

        }

        public void OnAllPropertyChanged()
        {
            OnPropertyChanged("CardList");
            OnPropertyChanged("SelectedCardsList");
            OnPropertyChanged("GameReport");
            OnPropertyChanged("CardsToTake");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Cards management methods
        public void Sort(SortCriteria criterium)
        {
            ActualPlayer.Sort(criterium);
            OnPropertyChanged("CardList");
        }
        public void HandOutCards(int numberOfCardsPerPlayers)
        {
            _lastCard = Deck.HandOutCards(numberOfCardsPerPlayers, PlayerList);
            GameReport += "Na stole leży " + _lastCard + "\n";
        }
        // do usuniecia niedlugo
        public void ThrowCard(int cardIndexToThrow)
        {
            Card cardToThrow = ActualPlayer.CardList.ElementAt(cardIndexToThrow);
            ActualPlayer.ThrowCards(cardToThrow);
            OnPropertyChanged("CardList");
        }
        public bool ThrowCard()
        {
            if(SelectedCardsList.Count() == 2)
            {
                MessageBox.Show("Niestety nie można rzucać 2 kart jednocześnie. Kart, które chcesz rzucić" +
                    " może być tylko 1, 3 lub 4.", "Błąd!");
                return false;
            }
            else if (Deck.CardsOnTheTableList.Count() > 0 
                && SelectedCardsList.ElementAt(0).Suit != Deck.CardsOnTheTableList.Last().Suit
                && SelectedCardsList.ElementAt(0).Value != Deck.CardsOnTheTableList.Last().Value)
            {
                MessageBox.Show("Niestety nie można rzucić kart ponieważ nie zgadza się " +
                    "kolor/wartość pierwszej z wybranych kart z kartą, która aktualnie jest na stole.",
                    "Błąd!");
                return false;
            }
            //else if(_lastCard.BattleCard.HasValue)
            else
            {
                string report = PlayerRound + " ";
                foreach (Card card in SelectedCardsList)
                {
                    int index = SelectedCardsList.ToList().IndexOf(card);
                    report += "rzucił " + card;
                    if (index == SelectedCardsList.Count() - 1)
                        report += ". \n";
                    else if (index > SelectedCardsList.Count() - 1)
                        report += ", ";
                    ActualPlayer.ThrowCards(card);
                    GameReport += report;

                    if (card.BattleCard.HasValue && CardsToTake == 1)
                        CardsToTake += (int)card.BattleCard - 1;
                    else if (card.BattleCard.HasValue)
                        CardsToTake += (int)card.BattleCard;
                    _lastCard = card;
                }
                ActualPlayer.ClearSelectedCards();
                OnAllPropertyChanged();
                return true;
            }
            
        }
        public bool SelectCard(int selectedIndex)
        {
            Card cardToSelect = CardList.ElementAt(selectedIndex);
            if (ActualPlayer.SelectedCardsList.Count() != 0 )
            {
                if (cardToSelect.Value != SelectedCardsList.ElementAt(0).Value)
                    return false;
                else
                {
                    ActualPlayer.SelectCard(cardToSelect);
                    OnAllPropertyChanged();
                    return true;
                }
            }
            else
            {
                ActualPlayer.SelectCard(cardToSelect);
                OnAllPropertyChanged();
                return true;
            }
            
        }
        public void ClearSelectedCards()
        {
            foreach (Card card in SelectedCardsList)
            {
                ActualPlayer.AddCard(card);
            }
            ActualPlayer.ClearSelectedCards();
            OnAllPropertyChanged();
        }
        #endregion

        
    }
}
