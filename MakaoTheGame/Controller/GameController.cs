﻿using System;
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
        public Player HumanPlayer { get => PlayerList[0]; } // DODANE - ADMIN MODE
        private int _roundNumber = 1;
        public Card _lastCard = null;
        public bool isSpecialCardUsed = false;
        private Values? _requestedValue = null;
        private Suits? _requestedSuit = null;
        private int _roundToWait = 0;
        private int _requestedValueRoundsLeft = 0;
        private IEnumerable<string> CardDeck { get => Deck.GetDeckCardNames(); }
        /// <summary>
        /// Set this attribute to true to change game mode to DEBUG MODE
        /// </summary>
        public bool AdminMode = true;
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
        public IEnumerable<Card> CardList
        {
            get
            {
                if (AdminMode)
                    return ActualPlayer.CardList;
                else
                    return HumanPlayer.CardList;
            }

        }
        public string ActualStateDescription
        {
            get
            {
                if (_requestedValueRoundsLeft > 0)
                    return "Żądanie karty " + _requestedValue.ToString() + ".";
                else if (CardsToTake > 1)
                    return "Kart do wzięcia: " + CardsToTake.ToString() + ".";
                else if (_lastCard.Value == Values.Ace)
                    return "Żądanie figury: " + SuitsExtensions.ToCustomSymbol((Suits)_requestedSuit) + ".";
                else
                    return "Aktualnie na stole: " + _lastCard.ToString() + "." ;
            }
        }
        public IEnumerable<Card> SelectedCardsList { get => ActualPlayer.SelectedCardsList.ToList(); }
        public string GameReport { get; private set; }
        public int CardsToTake { get; private set; }
        #endregion

        public GameController()
        {
            PlayerList.Add(new Player());
            PlayerList.Add(new AIPlayer(this));
            PlayerList.Add(new AIPlayer(this));
            GameReport += "Zaczynajmy grę!\n";
            CardsToTake = 1;
        }

        #region Help methods
        public void OnAllPropertyChanged()
        {
            OnPropertyChanged("CardList");
            OnPropertyChanged("SelectedCardsList");
            OnPropertyChanged("GameReport");
            OnPropertyChanged("CardsToTake");
            OnPropertyChanged("PlayerRound");
            OnPropertyChanged("ActualStateDescription");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool CheckAdvancedBattleCardConditions(Card firstSelected)
        {
            Card lastCard = _lastCard;
            if (lastCard.BattleCard.HasValue && CardsToTake != 1)
            {
                if (!firstSelected.BattleCard.HasValue)
                    return false;
                if (firstSelected.Value != lastCard.Value && firstSelected.Suit != lastCard.Suit
                    && firstSelected.Value != lastCard.Value + 1 && firstSelected.Value != lastCard.Value - 1)
                    return false;
                else return true;

            }

            return true;
        }
        public bool CheckAdvancedSpecialCardConditions(Card firstSelected)
        {
            Card lastCard = _lastCard;

            if (_requestedValue.HasValue)
            {
                Values reqValue = (Values)_requestedValue;
                if (firstSelected.Value != reqValue && firstSelected.Value != Values.Jack)
                    return false;
            }
            if (_requestedSuit.HasValue)
            {
                if (firstSelected.Suit != _requestedSuit)
                    return false;
            }
            if (isSpecialCardUsed)
            {
                //if (firstSelected.Value != Values.Four)
                //{
                // _isSpecialCardUsed = false;
                if(firstSelected.Value != Values.Four)
                return false;
                //}

            }
            return true;
        }
        public bool CheckBasicCardConditions(Card firstSelected)
        {
            if (_requestedSuit.HasValue)
            {
                if (firstSelected.Suit == _requestedSuit || firstSelected.Value == Values.Ace)
                    return true;
                return false;
            }
            else if (_requestedValue.HasValue)
            {
                if (firstSelected.Value == _requestedValue || firstSelected.Value == Values.Jack)
                    return true;
                return false;
            }
            else
            {
                if (Deck.CardsOnTheTableList.Count() > 0
                && firstSelected.Suit != Deck.CardsOnTheTableList.Last().Suit
                && firstSelected.Value != Deck.CardsOnTheTableList.Last().Value)
                    return false;
            }
            return true;
        }
        private bool CheckIfTheCardIsAce(Card cardToCheck)
        {
            if (cardToCheck.Value == Values.Ace)
            {
                if(!(ActualPlayer is AIPlayer))
                {
                    CardSelect cardSelector = new CardSelect(this);
                    cardSelector.ShowDialog();
                    GameReport += PlayerRound + " żąda " + SuitsExtensions.ToCustomSymbol((Suits)_requestedSuit)
                        + ".\n";
                    return true;
                }
                else
                {
                    AIPlayer aiPlayer = ActualPlayer as AIPlayer;
                    aiPlayer.ShuffleCards();
                    Dictionary<Suits, int> SuitsCounter = new Dictionary<Suits, int>();
                    for (int i = 0; i < 4; i++)
                        SuitsCounter.Add((Suits)i, 0);
                    foreach (Card card in ActualPlayer.CardList.ToList())
                    {
                        SuitsCounter[card.Suit]++;
                    }
                    //try
                    //{
                    Suits selectedSuit =(Suits) SuitsCounter.ToList().FindIndex(x => x.Value == SuitsCounter.Values.Max()) /*SuitsCounter.Max(x => x.Key)*/;
                    //}
                    // catch()

                    SetRequestedSuit(selectedSuit);
                    GameReport += PlayerRound + " żąda " + SuitsExtensions.ToCustomSymbol((Suits)_requestedSuit)
                       + ".\n";
                }
                
            }
            return false;
        }
        private bool CheckIfTheCardIsJack(Card cardToCheck)
        {
            if (cardToCheck.Value == Values.Jack)
            {
                if(!(ActualPlayer is AIPlayer))
                {
                    CardSelectJack cardSelector = new CardSelectJack(this);
                    cardSelector.ShowDialog();
                    if (_requestedValue.HasValue)
                        GameReport += PlayerRound + " żąda " + _requestedValue + ".\n";
                    else
                        GameReport += PlayerRound + " nie żąda niczego.\n";

                    return true;
                }
                else
                {
                    AIPlayer aiPlayer = ActualPlayer as AIPlayer;
                    aiPlayer.ShuffleCards();
                    foreach (Card card in ActualPlayer.CardList.ToList())
                    {
                        if (card.SpecialCard == null && card.BattleCard == null && card.Value != Values.King)
                        {
                            SetRequestedValue(card.Value);
                            break;
                        }
                    }
                    if (_requestedValue.HasValue)
                        GameReport += PlayerRound + " żąda " + _requestedValue + ".\n";
                    else
                        GameReport += PlayerRound + " nie żąda niczego.\n";
                    return true;
                }
                
            }
            return false;
        }
        private void RaiseNumberOfCardsToTakeIfNeeded(Card cardToThrow)
        {
            if (cardToThrow.BattleCard.HasValue && CardsToTake == 1)
                CardsToTake += (int)cardToThrow.BattleCard - 1;
            else if (cardToThrow.BattleCard.HasValue)
                CardsToTake += (int)cardToThrow.BattleCard;
            _lastCard = cardToThrow;
        }
        private void RaiseNumberOfRoundToWaitIfNeeded(Card cardToThrow)
        {
            if (cardToThrow.Value == Values.Four)
            {
                _roundToWait++;
                isSpecialCardUsed = true;
            }
            else
            {
                _roundToWait = 0;
                isSpecialCardUsed = false;
            }
        }
        public void SetRequestedSuit(Suits requestedSuit)
        {
            _requestedSuit = requestedSuit;
        }
        public void SetRequestedValue(Values requestedValue)
        {
            _requestedValue = requestedValue;
            _requestedValueRoundsLeft = PlayerList.Count;
        }
        #endregion

        #region Basic game mechanics controler
        public void NextRound(bool takeCards)
        {
            string report = null;
            if (takeCards)
            {
                Card nextCardFromDeck = Deck.PreviewTheCardFromDeck();
                if ((!CheckBasicCardConditions(nextCardFromDeck) ||
                    (!CheckAdvancedBattleCardConditions(nextCardFromDeck))
                    || !CheckAdvancedSpecialCardConditions(nextCardFromDeck))
                    || ActualPlayer.PauseRoundCounter != 0)
                {
                    if (isSpecialCardUsed) // Jeżeli ostatni gracz rzucił 4 i aktualny gracz nie mial 4 aby odpowiedziec
                    {
                        isSpecialCardUsed = false;
                        ActualPlayer.PauseRoundCounter = _roundToWait;
                        report += PlayerRound + " będzie czekał " + _roundToWait.ToString() + " kolejek.\n";
                        _roundToWait = 0;
                    }
                    else if (ActualPlayer.PauseRoundCounter > 0) // Jeżeli aktualny gracz pauzuje
                    {
                        ActualPlayer.PauseRoundCounter--;
                        report += PlayerRound + " będzie czekał jeszcze " +
                            ActualPlayer.PauseRoundCounter.ToString() + " kolejek. \n";
                    }
                    else // W innym przypadku bierze odpowiednia ilosc kart.
                    {
                        ActualPlayer.TakeCards(CardsToTake);
                        report += PlayerRound + " pobrał " + CardsToTake + " kart.\n";
                        CardsToTake = 1;
                    }
                }
                else
                {
                    MessageBoxResult? reply = null;
                    if(!(ActualPlayer is AIPlayer))
                    {
                        reply = MessageBox.Show("Czy chcesz rzucić pierwszą z dobieranych kart (" +
                        nextCardFromDeck + ")?",
                        "Czy chcesz rzucić?", MessageBoxButton.YesNo);
                    }
                    
                    if (reply == MessageBoxResult.Yes || ActualPlayer is AIPlayer)
                    {
                        ActualPlayer.TakeCards(1);
                        ThrowCard(nextCardFromDeck);
                        _requestedSuit = null;
                       
                        CheckIfTheCardIsAce(nextCardFromDeck);
                        CheckIfTheCardIsJack(nextCardFromDeck);
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
            if (_requestedValueRoundsLeft > 0)
                _requestedValueRoundsLeft--;
            else
                _requestedValue = null;
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
            if (!CheckBasicCardConditions(cardToThrow) || (!CheckAdvancedBattleCardConditions(cardToThrow))
                || !CheckAdvancedSpecialCardConditions(cardToThrow))
                return false;
            else
            {
                string report = PlayerRound + " ";
                report += "rzucil pierwszą dobraną kartę: " + cardToThrow + ".\n";
                ActualPlayer.ThrowCards(cardToThrow);
                RaiseNumberOfCardsToTakeIfNeeded(cardToThrow);
                RaiseNumberOfRoundToWaitIfNeeded(cardToThrow);
                GameReport += report;
                OnAllPropertyChanged();
                CheckEndTheGame();
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
            if (ActualPlayer.PauseRoundCounter > 0)
            {
                MessageBox.Show("Niestety musisz pauzować obecną kolejkę.");
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
            else if (!CheckAdvancedBattleCardConditions(firstSelected) && CardsToTake != 1)
            {
                MessageBox.Show("Niestety przeciwnik rzucił kartę bitewną. Przebij ją inną, pasującą " +
                    "kartą bitewną, lub w razie braku dobierz odpowiednią ilość kart.");
                return false;
            }
            else if (!CheckAdvancedSpecialCardConditions(firstSelected))
            {
                MessageBox.Show("Niestety przeciwnik rzucił kartę specjalną. Przebij ją inną, pasującą " +
                    "kartą specjalną, lub spełnij żądanie.");
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
                    RaiseNumberOfRoundToWaitIfNeeded(card);
                }
                _requestedSuit = null;
                GameReport += report;

                CheckIfTheCardIsAce(firstSelected);
                CheckIfTheCardIsJack(firstSelected);
                ActualPlayer.ClearSelectedCards();
                CheckEndTheGame();
                OnAllPropertyChanged();
                return true;
            }

        }
        public bool SelectCard(int selectedIndex)
        {
            Card cardToSelect = ActualPlayer.CardList.ElementAt(selectedIndex);
            if (ActualPlayer.SelectedCardsList.Count() != 0)
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

        public bool PlayAsAIIfNeeded()
        {
            AIPlayer ComputerPlayer = ActualPlayer as AIPlayer;

            if (ComputerPlayer.SelectCardAI())
            {
                if (!ThrowCard())
                {
                    ComputerPlayer.TakeCards(CardsToTake);
                    return true;
                }
                return false;
            }

            return true;

        }

    }
}
