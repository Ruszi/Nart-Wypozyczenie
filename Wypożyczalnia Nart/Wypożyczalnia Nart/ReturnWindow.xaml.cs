using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using static Wypożyczalnia_Nart.equipmentWindow;

namespace Wypożyczalnia_Nart
{
    /// <summary>
    /// Logika interakcji dla klasy ReturnWindow.xaml
    /// </summary>

    public partial class ReturnWindow : Window
    {
        private List<Rental> rentalHistory;
        private List<Equipment> equipmentList;

        public ReturnWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // Wczytaj historię wypożyczeń
            if (File.Exists("rental_history.json"))
            {
                string json = File.ReadAllText("rental_history.json");
                rentalHistory = JsonSerializer.Deserialize<List<Rental>>(json) ?? new();
                cbRentalHistory.ItemsSource = rentalHistory;
                cbRentalHistory.DisplayMemberPath = null;
            }

            equipmentList = EquipmentDataHelper.LoadEquipment();
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            if (cbRentalHistory.SelectedItem is not Rental selectedRental)
            {
                MessageBox.Show("Wybierz sprzęt do zwrotu.");
                return;
            }

            var equipment = equipmentList.FirstOrDefault(e => e.Name == selectedRental.EquipmentName);
            if (equipment != null)
            {
                equipment.Quantity++;
                equipment.Status = "Dostępny";

                // Zapisz aktualizowany sprzęt
                File.WriteAllText("equipment.json", JsonSerializer.Serialize(equipmentList, new JsonSerializerOptions { WriteIndented = true }));

                // Usuń z historii (lub zostaw, jeśli chcesz logować wypożyczenia)
                rentalHistory.Remove(selectedRental);
                File.WriteAllText("rental_history.json", JsonSerializer.Serialize(rentalHistory, new JsonSerializerOptions { WriteIndented = true }));

                MessageBox.Show($"Zwrócono {equipment.Name}.", "Zwrot udany");
                this.Close();
            }
        }

        private void cbRentalHistory_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // opcjonalnie: wyświetl więcej informacji
        }
    }
}
