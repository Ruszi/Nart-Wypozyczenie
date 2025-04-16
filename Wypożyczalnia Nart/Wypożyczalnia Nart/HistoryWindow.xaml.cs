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
    /// Logika interakcji dla klasy HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow : Window
    {
        private const string FilePath = "rental_history.json";

        public HistoryWindow()
        {
            InitializeComponent();
            LoadHistory();
        }

        private void LoadHistory()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                var history = JsonSerializer.Deserialize<List<Rental>>(json);
                {
                    HistoryDataGrid.ItemsSource = history;
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // lub NavigateBack, jeśli masz NavigationWindow
        }

        private void CleanOrphanedHistory()
        {
            if (!File.Exists("equipment.json") || !File.Exists("rental_history.json"))
                return;

            var allEquipment = EquipmentDataHelper.LoadEquipment();
            var equipmentNames = allEquipment.Select(e => e.Name).ToHashSet();

            string json = File.ReadAllText("rental_history.json");
            var history = JsonSerializer.Deserialize<List<Rental>>(json) ?? new List<Rental>();

            var cleanedHistory = history.Where(r => equipmentNames.Contains(r.EquipmentName)).ToList();

            File.WriteAllText("rental_history.json", JsonSerializer.Serialize(cleanedHistory, new JsonSerializerOptions { WriteIndented = true }));

            MessageBox.Show("Usunięto osierocone wpisy z historii.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);

            // Przeładuj widok
            LoadHistory();
        }
        private void CleanOrphaned_Click(object sender, RoutedEventArgs e)
        {
            CleanOrphanedHistory();
        }


    }
}
