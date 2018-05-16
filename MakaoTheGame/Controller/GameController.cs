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
        #region Help methods
        public void OnAllPropertyChanged()
        {
            OnPropertyChanged("CardList");
            OnPropertyChanged("SelectedCardsList");
            OnPropertyChanged("GameReport");
            OnPropertyChanged("CardsToTake");
            OnPropertyChanged("PlayerRound");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        private bool CheckAdvancedBattleCardConditions(Card firstSelected)
        {
            Card lastCard = _lastCard;
            //Card firstSelected = SelectedCardsList.ElementAt(0);
            if (lastCard.BattleCard.HasValue)
            {
                if (!firstSelected.BattleCard.HasValue /*|| CardsToTake != 1*/)
                    return false;
                if (firstSelected.Value != lastCard.Value && firstSelected.Suit != lastCard.Suit
                    && firstSelected.Value != lastCard.Value + 1 && firstSelected.Value != lastCard.Value - 1)
                    return false;
                else return true;

            }

            return true;
        }
        private bool CheckBasicCardConditions(Card firstSelected)
        {
            if (Deck.CardsOnTheTableList.Count() > 0
                && firstSelected.Suit != Deck.CardsOnTheTableList.Last().Suit
                && firstSelected.Value != Deck.CardsOnTheTableList.Last().Value)
                return false;
            return true;
        }
        private void RaiseNumberOfCardsToTakeIfNeeded(Card cardToThrow)
        {
            if (cardToThrow.BattleCard.HasValue && CardsToTake == 1)
                CardsToTake += (int)cardToThrow.BattleCard - 1;
            else if (cardToThrow.BattleCard.HasValue)
                CardsToTake += (int)cardToThrow.BattleCard;
            _lastCard = cardToThrow;
        }
        #endregion

        #region Basic game mechanics controler
        public void NextRound(bool takeCards)
        {
            string report = null;
            if(takeCards)
            {
                Card nextCardFromDeck = Deck.PreviewTheCardFromDeck();
                if(!CheckBasicCardConditions(nextCardFromDeck) || 
                    (!CheckAdvancedBattleCardConditions(nextCardFromDeck)))
                {
                    
                    ActualPlayer.TakeCards(CardsToTake);
                    report += PlayerRound + " pobrał " + CardsToTake + " kart.\n";
                    CardsToTake = 1;
                    
                }
                else
                {
                    MessageBoxResult reply = MessageBox.Show("Czy chcesz rzucić pierwszą z dobieranych kart (" +
                        nextCardFromDeck + ")?",
                        "Czy chcesz rzucić?", MessageBoxButton.YesNo);
                    if(reply == MessageBoxResult.Yes)
                    {
                        ActualPlayer.TakeCards(1);
                        ThrowCard(nextCardFromDeck);
                    }
                    else
                    {
                        ActualPlayer.TakeCards(CardsToTake);
                        report += PlayerRound + " pobrał " + CardsToTake + " kart.\n";
                        CardsToTake = 1;
                    }
                }
                if (report != null)
                    GameReport += report;
            }
            int kingSpadeIndnex = 12;
            _roundNumber++;
            if (_lastCard.CardIndex == kingSpadeIndnex && CardsToTake > 1)
                _roundNumber -= 2;
            if (_roundNumber > PlayerList.Count())
                _roundNumber = 1;
            else if (_roundNumber < 1)
                _roundNumber = 3;
            OnAllPropertyChanged();

        }

        public void ResetAll()
        {

        }
        private void CheckEndTheGame()
        {
            if (ActualPlayer.CardCount == 0)
                MessageBox.Show("Wygral gracz: " + PlayerRound);
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
        public bool ThrowCard(Card cardToThrow)
        {
            if (!CheckBasicCardConditions(cardToThrow) || (!CheckAdvancedBattleCardConditions(cardToThrow)))
                return false;
            else
            {
                string report = PlayerRound + " ";
                report += "rzucil pierwszą dobraną kartę: " + cardToThrow + ".\n";
                ActualPlayer.ThrowCards(cardToThrow);
                RaiseNumberOfCardsToTakeIfNeeded(cardToThrow);
                GameReport += report;
                OnAllPropertyChanged();
                return true;
            }
        }
        public bool ThrowCard()
        {
            if (SelectedCardsList.Count() == 0)
            {
                MessageBox.Show("Musisz wybrać jakieś karty zanim rzucisz cokolwiek.", "Wybierz karty!");
                return false;
            }
                
            Card firstSelected = SelectedCardsList.ToList().ElementAt(0);
            if (SelectedCardsList.Count() == 2)
            {
                MessageBox.Show("Niestety nie można rzucać 2 kart jednocześnie. Kart, które chcesz rzucić" +
                    " może być tylko 1, 3 lub 4.", "Błąd!");
                return false;
            }
            else if (!CheckBasicCardConditions(firstSelected))
            {
                MessageBox.Show("Niestety nie można rzucić kart ponieważ nie zgadza się " +
                    "kolor/wartość pierwszej z wybranych kart z kartą, która aktualnie jest na stole.",
                    "Błąd!");
                return false;
            }
            else if(!CheckAdvancedBattleCardConditions(firstSelected) && CardsToTake != 1)
            {
                MessageBox.Show("Niestety przeciwnik rzucił kartę bitewną. Przebij ją inną, pasującą " +
                    "kartą bitewną, lub w razie braku dobierz odpowiednią ilość kart.");
                return false;
            }
            else
            {
                string report = PlayerRound + " ";
                foreach (Card card in SelectedCardsList)
                {
                    int index = SelectedCardsList.ToList().IndexOf(card);
                    report += "rzucił " + card;
                    if (index == SelectedCardsList.Count() - 1)
                        report += ". \n";
                    else if (index < SelectedCardsList.Count() - 1)
                        report += ", ";
                    ActualPlayer.ThrowCards(card);

                    RaiseNumberOfCardsToTakeIfNeeded(card);
                }
                GameReport += report;
                ActualPlayer.ClearSelectedCards();
                CheckEndTheGame();
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
