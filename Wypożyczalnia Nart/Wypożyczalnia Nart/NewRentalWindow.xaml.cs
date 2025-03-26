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

namespace Wypożyczalnia_Nart
{
    /// <summary>
    /// Logika interakcji dla klasy NewRentalWindow.xaml
    /// </summary>
    public partial class NewRentalWindow : Window
    {
        public NewRentalWindow()
        {
            InitializeComponent();
        }


        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text;
            string equipment = cbEquipment.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(equipment))
            {
                MessageBox.Show("Wypełnij wszystkie pola!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show($"Wypożyczono {equipment} dla {name}!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }



    }




}
