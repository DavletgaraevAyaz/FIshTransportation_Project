using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FIshTransportation.Scripts;
using Test22.Scripts;

namespace FIshTransportation
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int transportTime; // Переменная для хранения времени перевозки

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                transportTime = int.Parse(TransportTimeTextBox.Text); // Сохраняем время перевозки
                int minTemp = int.Parse(MinTempTextBox.Text);
                int maxTemp = int.Parse(MaxTempTextBox.Text);
                var temperatures = TemperatureInputTextBox.Text.Split(',').Select(int.Parse).ToArray();

                var monitor = new TemperatureMonitor(transportTime, minTemp, maxTemp, temperatures);
                var report = monitor.CheckConditions();

                ResultTextBlock.Text = report;
                SaveReportToFile(report);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void LoadFromFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog // Используем OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var (startTime, temperatures) = DataLoader.LoadDataFromFile(openFileDialog.FileName);
                // Установите время перевозки (например, можно установить его на основе времени доставки)
                // Здесь можно добавить логику для определения времени перевозки
                TransportTimeTextBox.Text = "0"; // Установите значение по умолчанию или определите его
                TemperatureInputTextBox.Text = string.Join(", ", temperatures); // Установите температуры
                // Можно добавить логику для установки минимальной и максимальной температуры
            }
        }

        private void ItemComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ItemComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                switch (selectedItem.Content.ToString())
                {
                    case "Семга":
                        MinTempTextBox.Text = "-3"; // Минимальная температура для семги
                        MaxTempTextBox.Text = "5";   // Максимальная температура для семги
                        break;
                    case "Минтай":
                        MinTempTextBox.Text = "-10"; // Минимальная температура для минтая
                        MaxTempTextBox.Text = "-5";   // Максимальная температура для минтая
                        break;
                }
            }
        }

        private void SaveReportToFile(string report)
        {
            string filePath = "report.txt";
            File.WriteAllText(filePath, report);
            MessageBox.Show($"Отчет сохранен в {filePath}");
        }

        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверка, что вводимое значение является числом
            e.Handled = !IsTextAllowed(e.Text);
        }

        private bool IsTextAllowed(string text)
        {
            // Регулярное выражение для проверки, что текст состоит только из цифр
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
        }

        private void TransportTimeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(TransportTimeTextBox.Text, out int value))
            {
                if (value < 3 || value > 9)
                {
                    MessageBox.Show("Срок доставки должен быть от 3 до 9 часов.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void MinTempTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MinTempTextBox.Text, out int value))
            {
                // Здесь можно добавить дополнительные проверки для минимальной температуры, если необходимо
            }
        }

        private void MaxTempTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MaxTempTextBox.Text, out int value))
            {
                // Здесь можно добавить дополнительные проверки для максимальной температуры, если необходимо
            }
        }
    }
}
