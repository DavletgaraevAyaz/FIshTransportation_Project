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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int maxTemp = int.Parse(MaxTempTextBox.Text);
                int maxTempDuration = int.Parse(MaxTempDurationTextBox.Text);
                int minTemp = int.Parse(MinTempTextBox.Text);
                int minTempDuration = int.Parse(MinTempDurationTextBox.Text);
                var temperatures = TemperatureInputTextBox.Text.Split(',').Select(int.Parse).ToArray();
                DateTime startTime = DateTime.Parse(StartTimeTextBox.Text);

                var monitor = new TemperatureMonitor(startTime, minTemp, maxTemp, temperatures);
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
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var (startTime, temperatures) = DataLoader.LoadDataFromFile(openFileDialog.FileName);
                StartTimeTextBox.Text = startTime.ToString("dd.MM.yyyy HH:mm");
                TemperatureInputTextBox.Text = string.Join(", ", temperatures);
            }
        }

        private void ItemComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ItemComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                switch (selectedItem.Content.ToString())
                {
                    case "Семга":
                        MinTempTextBox.Text = "-3"; 
                        MaxTempTextBox.Text = "5";  
                        MinTempDurationTextBox.Text = "60";
                        MaxTempDurationTextBox.Text = "20"; 
                        break;
                    case "Минтай":
                        MinTempTextBox.Text = "-10"; 
                        MaxTempTextBox.Text = "-5";   
                        MinTempDurationTextBox.Text = "0"; 
                        MaxTempDurationTextBox.Text = "120"; 
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
            e.Handled = !IsTextAllowed(e.Text);
        }

        private bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
        }

        private void MinTempTextBox_LostFocus(object sender, RoutedEventArgs e) {  }
        private void MaxTempTextBox_LostFocus(object sender, RoutedEventArgs e) {  }
        private void MinTempDurationTextBox_LostFocus(object sender, RoutedEventArgs e) {  }
        private void MaxTempDurationTextBox_LostFocus(object sender, RoutedEventArgs e) {  }
        private void Temperature_LostFocus(object sender, RoutedEventArgs e) 
        {
            if (int.TryParse(TemperatureInputTextBox.Text, out int value))
            {
                if (value < 18 || value > 54)
                {
                    MessageBox.Show("Значения температуры от 18 до 54.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}
