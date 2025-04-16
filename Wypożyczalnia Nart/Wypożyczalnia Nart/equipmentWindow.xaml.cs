using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.Win32;

namespace Wypożyczalnia_Nart
{
    /// <summary>
    /// Logika interakcji dla klasy equipmentWindow.xaml
    /// </summary>
    public class Equipment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public int Length { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = "Dostępny";
        public int Quantity { get; set; } = 1;
        public string ImagePath { get; set; } = "";

        public override string ToString()
        {
            return $"{Name} - {Length}cm, {Description}, {Price:C} zł, {Status}, Ilość: {Quantity}";
        }
    }

    public partial class equipmentWindow : Window
    {
        private ObservableCollection<Equipment> equipmentList = new ObservableCollection<Equipment>();
        private const string FilePath = "equipment.json";
        private const string LogFilePath = "equipment_log.txt";

        public equipmentWindow()
        {
            InitializeComponent();
            LoadEquipment();
            EquipmentDataGrid.ItemsSource = equipmentList;
        }

        private void LoadEquipment()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                var items = JsonSerializer.Deserialize<ObservableCollection<Equipment>>(json);
                if (items != null)
                {
                    equipmentList = items;
                }
            }
        }

        private void SaveEquipment()
        {
            string json = JsonSerializer.Serialize(equipmentList);
            File.WriteAllText(FilePath, json);
        }

        private void LogChange(string message)
        {
            File.AppendAllText(LogFilePath, $"{DateTime.Now}: {message}\n");
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var name = Microsoft.VisualBasic.Interaction.InputBox("Podaj nazwę sprzętu:", "Dodaj sprzęt", "");
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Nazwa nie może być pusta!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var lengthStr = Microsoft.VisualBasic.Interaction.InputBox("Podaj długość (cm, tylko liczby):", "Dodaj sprzęt", "");
            if (!int.TryParse(lengthStr, out int length) || length <= 0)
            {
                MessageBox.Show("Długość musi być liczbą większą od zera!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            var description = Microsoft.VisualBasic.Interaction.InputBox("Podaj opis:", "Dodaj sprzęt", "");
            var priceStr = Microsoft.VisualBasic.Interaction.InputBox("Podaj cenę (zł, tylko liczby):", "Dodaj sprzęt", "");
            if (!decimal.TryParse(priceStr, out decimal price) || price <= 0)
            {
                MessageBox.Show("Cena musi być liczbą większą od zera!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var quantityStr = Microsoft.VisualBasic.Interaction.InputBox("Podaj ilość:", "Dodaj sprzęt", "1");
            if (!int.TryParse(quantityStr, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Ilość musi być liczbą większą od zera!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            

            var newEquipment = new Equipment { Name = name, Length = length, Description = description, Price = price, Quantity = quantity };
            equipmentList.Add(newEquipment);
            SaveEquipment();
            LogChange($"Dodano sprzęt: {newEquipment.Name}");
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (EquipmentDataGrid.SelectedItem is Equipment selected)
            {
                var newName = Microsoft.VisualBasic.Interaction.InputBox("Podaj nową nazwę:", "Edytuj sprzęt", selected.Name);
                if (!string.IsNullOrWhiteSpace(newName)) selected.Name = newName;

                var newLengthStr = Microsoft.VisualBasic.Interaction.InputBox("Podaj nową długość:", "Edytuj sprzęt", selected.Length.ToString());
                if (int.TryParse(newLengthStr, out int newLength) && newLength > 0) selected.Length = newLength;

                SaveEquipment();
                LogChange($"Edytowano sprzęt: {selected.Name}");
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (EquipmentDataGrid.SelectedItem is Equipment selected)

            {
                // Zapytanie o potwierdzenie przed wprowadzeniem hasła
                MessageBoxResult confirmResult = MessageBox.Show($"Czy na pewno chcesz usunąć {selected.Name}?", "Potwierdzenie usunięcia", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (confirmResult == MessageBoxResult.Yes)
                {
                    // Okno dialogowe do wprowadzenia hasła
                    var password = Microsoft.VisualBasic.Interaction.InputBox("Podaj hasło, aby potwierdzić usunięcie:", "Weryfikacja hasła", "");

                    // Sprawdzenie, czy hasło jest poprawne
                    if (password == "1234") // Zmień na swoje hasło
                    {
                        // Usunięcie sprzętu, jeśli hasło jest poprawne
                        equipmentList.Remove(selected);
                        SaveEquipment();
                        LogChange($"Usunięto sprzęt: {selected.Name}");
                        MessageBox.Show($"Sprzęt {selected.Name} został usunięty.", "Usunięcie zakończone", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        // Błąd, jeśli hasło jest niepoprawne
                        MessageBox.Show("Niepoprawne hasło! Sprzęt nie został usunięty.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

   


        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            string keyword = Microsoft.VisualBasic.Interaction.InputBox("Wpisz nazwę do wyszukania:", "Filtruj sprzęt", "");
            EquipmentDataGrid.ItemsSource = equipmentList.Where(e => e.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            equipmentList = new ObservableCollection<Equipment>(equipmentList.OrderBy(e => e.Name));
            EquipmentDataGrid.ItemsSource = equipmentList;

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        public static class EquipmentDataHelper
        {
            private const string FilePath = "equipment.json";

            public static List<Equipment> LoadEquipment()
            {
                if (File.Exists(FilePath))
                {
                    string json = File.ReadAllText(FilePath);
                    var items = JsonSerializer.Deserialize<List<Equipment>>(json);
                    return items ?? new List<Equipment>();
                }
                return new List<Equipment>();
            }
        }

    }
}