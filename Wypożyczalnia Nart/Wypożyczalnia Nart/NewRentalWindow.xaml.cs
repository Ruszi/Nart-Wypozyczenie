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
using static Wypożyczalnia_Nart.equipmentWindow;
using System.IO;

namespace Wypożyczalnia_Nart
{
    /// <summary>
    /// Logika interakcji dla klasy NewRentalWindow.xaml
    /// </summary>
    public partial class NewRentalWindow : Window
    {
        private List<Equipment> allEquipment;

        public NewRentalWindow()
        {
            InitializeComponent();
            LoadEquipmentList();
        }

        private void LoadEquipmentList()
        {
            allEquipment = EquipmentDataHelper.LoadEquipment();

            var availableEquipment = allEquipment
                .Where(e => e.Status == "Dostępny" && e.Quantity > 0)
                .ToList();

            cbEquipment.ItemsSource = availableEquipment;
            cbEquipment.DisplayMemberPath = "Name"; // wyświetla nazwę, ale przechowuje cały obiekt
        }

        private void cbEquipment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbEquipment.SelectedItem is Equipment selectedEquipment)
            {
                txtLength.Text = $"{selectedEquipment.Length} cm";
                txtPrice.Text = $"{selectedEquipment.Price} zł";
                txtDescription.Text = selectedEquipment.Description;
            }
            else
            {
                txtLength.Text = "";
                txtPrice.Text = "";
                txtDescription.Text = "";
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text;
            if (cbEquipment.SelectedItem is not Equipment selectedEquipment || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Wypełnij wszystkie pola!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var item = allEquipment.FirstOrDefault(eq => eq.Id == selectedEquipment.Id);
            if (item != null)
            {
                item.Quantity--;
                if (item.Quantity == 0)
                    item.Status = "Niedostępny";

                File.WriteAllText("equipment.json", JsonSerializer.Serialize(allEquipment));

                // ➕ Zapis historii
                var rental = new Rental
                {
                    CustomerName = name,
                    EquipmentName = item.Name,
                    Date = DateTime.Now
                };

                string historyFile = "rental_history.json";
                List<Rental> history = new();

                if (File.Exists(historyFile))
                {
                    string existingData = File.ReadAllText(historyFile);
                    var existingHistory = JsonSerializer.Deserialize<List<Rental>>(existingData);
                    if (existingHistory != null)
                        history = existingHistory;
                }

                history.Add(rental);

                string updatedJson = JsonSerializer.Serialize(history, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(historyFile, updatedJson);

                MessageBox.Show($"Wypożyczono {item.Name} dla {name}!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Wybrany sprzęt nie jest już dostępny.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




    }
}
