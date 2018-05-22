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
    /// Logika interakcji dla klasy CardSelectJack.xaml
    /// </summary>
    public partial class CardSelectJack : Window
    {
        private GameController _gameController { get; set; }
        internal CardSelectJack(GameController gameController)
        {
            InitializeComponent();
            _gameController = gameController;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int valueSelected = int.Parse(((Button)sender).Tag.ToString());
            if (valueSelected != 0)
                _gameController.SetRequestedValue((Values)valueSelected);
            this.Close();
        }
    }
}
