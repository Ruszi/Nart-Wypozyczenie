using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logika interakcji dla klasy equipmentWindow.xaml
    /// </summary>
    public partial class equipmentWindow : Window
    {
        private ObservableCollection<string> equipmentList = new ObservableCollection<string>
        {
            "Narty Rossignol",
            "Narty Atomic",
            "Narty Fischer"
        };

        public equipmentWindow()
        {
            InitializeComponent();
            EquipmentListBox.ItemsSource = equipmentList; // Corrected ListBox reference
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            equipmentList.Add("Nowy sprzęt");
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (EquipmentListBox.SelectedItem != null)
            {
                equipmentList.Remove(EquipmentListBox.SelectedItem.ToString());
            }
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}