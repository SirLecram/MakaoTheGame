using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MakaoTheGame.Controller;

namespace MakaoTheGame
{
    /// <summary>
    /// Aktualnie w trakcie:
    /// Ostatnio dodane: ADMIN MODE, naprawione rzucanie 4 na 4 przez komputer, raport bind
    /// Do poprawy:
    /// 
    /// 
    /// Działa: Zarys klasy player, dziala rozdawanie kart, Bindowanie listy kart uzytkownika,
    /// Pobieranie odpowiedniej ilości kart z kupki, przetasowanie kart w razie ich braku w kupce, 
    /// czyszczenie kart wybranych, Zaawansowane warunki rzucania kart (BITEWNE),
    /// warunki wybierania kart, sprawdzanie podstawowych warunków rzuciania kart,
    /// dodanie do kart symboli, bindowanie części informacji o rundzie, wstepnie dziala raport,
    /// sortowanie kart, przeciwnicy (kontrolowani przez gracza), atrybuty kart bitewnych/specjalnych,
    /// podstawowe i zaawansowane dzialanie kart bitewnych, rzucanie kart na stół, raport autoscroll
    /// sprawdzenie pierwszej karty z kupki przed dobieraniem, dzialane Króla pik, sprawdzanie zwyciestwa.
    /// zaawansowane warunki rzucania (czwórka, ace, jopek), okno wyboru, 
    /// dzialanie kart specjalnych, AIPlayer, AI wybieranie żadania, koloru, rzucanie dobranej karty,
    /// AIPlayer potrafi decydować jaką kartę żądać w przypadku jopka/asa, AI potrafi
    /// rzucic pierwszą dobraną kartę, WYBIERANIE PRZEZ AI KOLORU PO RZUCENIU ASA,
    /// AI decyduje o kolorze karty po asie na podstawie ilosci kart w rece.
    /// Do zrobienia niedlugo: 
    /// PRZECIWNICY AI, Rozgrywka, zmiana kolejności wybranych kart, 
    /// makao, reset gry, TESTY!!!
    /// BŁĘDY: propozycja rzucenai karty w przypadku
    /// gdy król bitewny jest na spodzie rzucanych kilku kart.
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameController GameController = new GameController();
        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        #region Events
        private void NextRoundBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GameController.ActualPlayer is Model.AIPlayer)
            {
                GameController.NextRound(GameController.PlayAsAIIfNeeded());
            }
            else
            {
                GameController.NextRound(true);
                GameController.ClearSelectedCards();
            }

            SetRightButtonVisibility();
            CardsSort();
            ScrollReportTextBoxToEnd();
        }

        private void ThrowSelectedCardsButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCardsListBox.Items != null)
            {
                if (GameController.ThrowCard())
                    GameController.NextRound(false);
            }
            CardsSort();
            ScrollReportTextBoxToEnd();
        }

        private void SortCardsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CardsSort();
        }

        private void SelectChoosenCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (playerCardsListBox.SelectedItem != null)
            {
                int index = playerCardsListBox.SelectedIndex;
                if(!GameController.SelectCard(index))
                {
                    MessageBox.Show("Nie można wybrać tej karty. Kart jest za dużo lub nie zgadza się " +
                        "wartość karty z kartami wybranymi wcześniej.");
                }

            }
        }

        private void ClearSelectedCardsButton_Click(object sender, RoutedEventArgs e)
        {
            GameController.ClearSelectedCards();
            CardsSort();
        }
        #endregion

        #region Help methods
        private void CardsSort()
        {
            SortCriteria criterium = (SortCriteria)sortCardsComboBox.SelectedIndex;
            GameController.Sort(criterium);
        }
        private void InitializeGame()
        {
            GameController.HandOutCards(15);
            InitializeBinding();
        }
        private void InitializeBinding()
        {
            sortCardsComboBox.ItemsSource = SortCriteriaExtensions.GetNamesList();
            sortCardsComboBox.SelectedIndex = 0;
            playerCardsListBox.DataContext = GameController;
            selectedCardsListBox.DataContext = GameController;
            actualTurnTextBox.DataContext = GameController;
            gameTextBox.DataContext = GameController;
            cardToTakeTextBox.DataContext = GameController;
            actualStateDescription.DataContext = GameController;
            CardsSort();
        }
        private void ScrollReportTextBoxToEnd()
        {
            gameTextBox.Focus();
            gameTextBox.CaretIndex = gameTextBox.Text.Length;
            gameTextBox.ScrollToEnd();
        }
        private void SetRightButtonVisibility()
        {
            if(GameController.ActualPlayer is Model.AIPlayer && !GameController.AdminMode)
            {
                clearSelectedCardsButton.IsEnabled = false;
                dropSelectedCardsButton.IsEnabled = false;
                selectChoosenCardButton.IsEnabled = false;
                playerCardsListBox.IsEnabled = false;
                selectedCardsListBox.IsEnabled = false;
            }
            else
            {
                clearSelectedCardsButton.IsEnabled = true;
                dropSelectedCardsButton.IsEnabled = true;
                selectChoosenCardButton.IsEnabled = true;
                playerCardsListBox.IsEnabled = true;
                selectedCardsListBox.IsEnabled = true;
            }
        }
        #endregion
    }
}
