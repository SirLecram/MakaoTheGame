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
using System.Windows.Shapes;
using MakaoTheGame.Controller;

namespace MakaoTheGame.Model
{
    /// <summary>
    /// Logika interakcji dla klasy CardSelect.xaml
    /// </summary>
    public partial class CardSelect : Window
    {
        private GameController _gameController { get; set; }
        internal CardSelect(GameController gameController)
        {
            InitializeComponent();
            _gameController = gameController;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _gameController.SetRequestedSuit((Suits)int.Parse(((Button)sender).Tag.ToString()));
            this.Close();
        }
    }
}
