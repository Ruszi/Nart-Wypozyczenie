using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wypożyczalnia_Nart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Equipment_Click (object sender, RoutedEventArgs e)
        {
            equipmentWindow oknoSprzet = new equipmentWindow();
            oknoSprzet.ShowDialog();
        }

        private void NewRental_Click(object sender, RoutedEventArgs e)
        {
            NewRentalWindow oknoNoweWypozyczenie = new NewRentalWindow();
            oknoNoweWypozyczenie.ShowDialog();
        }


    }
}