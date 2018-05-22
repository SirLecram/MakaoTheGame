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
    /// Aktualnie w trakcie: zaawansowane warunki rzucania (specjalne JOPEK)
    /// Ostatnio dodane: zaawansowane warunki rzucania (czwórka, ace), okno wyboru,
    /// Działa: Zarys klasy player, dziala rozdawanie kart, Bindowanie listy kart uzytkownika,
    /// Pobieranie odpowiedniej ilości kart z kupki, przetasowanie kart w razie ich braku w kupce, 
    /// czyszczenie kart wybranych, Zaawansowane warunki rzucania kart (BITEWNE),
    /// warunki wybierania kart, sprawdzanie podstawowych warunków rzuciania kart,
    /// dodanie do kart symboli, bindowanie części informacji o rundzie, wstepnie dziala raport,
    /// sortowanie kart, przeciwnicy (kontrolowani przez gracza), atrybuty kart bitewnych/specjalnych,
    /// podstawowe i zaawansowane dzialanie kart bitewnych, rzucanie kart na stół, raport autoscroll
    /// sprawdzenie pierwszej karty z kupki przed dobieraniem, dzialane Króla pik, sprawdzanie zwyciestwa.
    /// 
    /// Do zrobienia niedlugo: Klasa AIPlayer, 
    /// PRZECIWNICY AI, Rozgrywka, zmiana kolejności wybranych kart, dzialanie kart specjalnych,
    /// makao, reset gry, 
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
        //Na razie dzialanie tego przycisku to TEST.
        private void NextRoundBtn_Click(object sender, RoutedEventArgs e)
        {
            GameController.ClearSelectedCards();
            GameController.NextRound(true);
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
            GameController.HandOutCards(7);
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
            CardsSort();
        }
        private void ScrollReportTextBoxToEnd()
        {
            gameTextBox.Focus();
            gameTextBox.CaretIndex = gameTextBox.Text.Length;
            gameTextBox.ScrollToEnd();
        }
        #endregion
    }
}
