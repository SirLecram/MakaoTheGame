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
    /// OStatnie zmiany!! Zarys klasy player, wstępnie dziala rozdawanie kart, Bindowanie listy kart uzytkownika,
    /// Pobieranie kart z kupki, przetasowanie kart w razie ich braku w kupce, dodanie do kart symboli,
    /// (wstępnie)rzucanie kart na stół, sortowanie kart
    /// Do zrobienia niedlugo: Klasa AIPlayer,  wybieranie kilku kart z reki oraz rzucanie ich na stół, 
    /// TextBox z przebiegiem gry, PRZECIWNICY, Rozgrywka, warunki rzucania kart.
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameController GameController = new GameController();
        public MainWindow()
        {
            InitializeComponent();
            sortCardsComboBox.ItemsSource = SortCriteriaExtensions.GetNamesList();
            sortCardsComboBox.SelectedIndex = 0;
            GameController.HandOutCards(7);
            playerCardsListBox.DataContext = GameController;
            // playerCardsListBox.ItemsSource = ;
            selectedCardsListBox.DataContext = GameController;
            //selectedCardsListBox.ItemsSource = 
        }

        private void NextRoundBtn_Click(object sender, RoutedEventArgs e)
        {
            GameController.NextRound();
            
        }

        private void ThrowSelectedCardsButton_Click(object sender, RoutedEventArgs e)
        {
            if(playerCardsListBox.SelectedItem != null)
            {
                int index = playerCardsListBox.SelectedIndex;
                GameController.ThrowCard(index);
            }  
        }

        private void SortCardsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortCriteria criterium = (SortCriteria)sortCardsComboBox.SelectedIndex;
            GameController.Sort(criterium);
        }

        private void SelectChoosenCardButton_Click(object sender, RoutedEventArgs e)
        {
            if(playerCardsListBox.SelectedItem != null)
            {
                int index = playerCardsListBox.SelectedIndex;
                GameController.SelectCard(index);
            }
            //UpdateSelectedCardsLabel();
        }

        private void UpdateSelectedCardsLabel()
        {
            selectedCardsListBox.ItemsSource = GameController.SelectedCardsList;
        }
    }
}
