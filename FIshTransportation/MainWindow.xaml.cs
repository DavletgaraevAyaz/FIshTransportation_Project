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

namespace FIshTransportation
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int transportTime;
        private Fish fish;
        private List<TemperatureRecord> temperatureRecords;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            fish = new Fish
            {
                Type = ItemComboBox.Text,
                MaxTemperature = int.Parse(MaxTempTextBox.Text),
                MaxTime = int.Parse(MaxTimeTExtBox.Text),
                MinTemperature = int.Parse(MinTempTextBox.Text),
                MinTime = int.Parse(MinTimeTextBox.Text)
            };

            // Проверка условий хранения
            var report = CheckStorageConditions(temperatureRecords, fish);
            DisplayReport(report);
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
                        break;
                    case "Минтай":
                        MinTempTextBox.Text = "-10"; 
                        MaxTempTextBox.Text = "-5";   
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

        private List<Report> CheckStorageConditions(List<TemperatureRecord> records, Fish fish)
        {
            var report = new List<Report>();
            DateTime startTime = DateTime.Parse(records.First().DateTime.ToString());
            DateTime endTime = DateTime.Parse(records.Last().DateTime.ToString());
            TimeSpan duration = endTime - startTime;

            int maxViolationTime = 0;
            int minViolationTime = 0;

            for (int i = 0; i < records.Count; i++)
            {
                int temp = records[i].Temperature;
                DateTime currentTime = DateTime.Parse(records[i].DateTime.ToString());

                if (temp > fish.MaxTemperature)
                {
                    maxViolationTime++;
                    if (maxViolationTime > fish.MaxTime)
                    {
                        report.Add(new Report
                        {
                            ViolationTime = currentTime,
                            RequiredTemperature = fish.MaxTemperature,
                            ActualTemperature = temp,
                            Deviation = temp - fish.MaxTemperature
                        });
                    }
                }
                else
                {
                    maxViolationTime = 0; // Reset if within limits
                }

                if (temp < fish.MinTemperature)
                {
                    minViolationTime++;
                    if (minViolationTime > fish.MinTime)
                    {
                        report.Add(new Report
                        {
                            ViolationTime = currentTime,
                            RequiredTemperature = fish.MinTemperature,
                            ActualTemperature = temp,
                            Deviation = temp - fish.MinTemperature
                        });
                    }
                }
                else
                {
                    minViolationTime = 0; // Reset if within limits
                }
            }

            return report;
        }

        private void DisplayReport(List<Report> report)
        {
            if (report.Count > 0)
            {
                NotificationTextBlock.Text = "Нарушения условий хранения зафиксированы.";
                ReportDataGrid.ItemsSource = report;
                SaveReportToFile(report);
            }
            else
            {
                NotificationTextBlock.Text = "Условия хранения соблюдены.";
            }
        }

        private void SaveReportToFile(List<Report> report)
        {
            using (StreamWriter writer = new StreamWriter("Report.txt"))
            {
                foreach (var entry in report)
                {
                    writer.WriteLine($"{entry.ViolationTime} | Требуемая: {entry.RequiredTemperature} | Фактическая: {entry.ActualTemperature} | Отклонение: {entry.Deviation}");
                }
            }
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
            }
        }

        private void MaxTempTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MaxTempTextBox.Text, out int value))
            {
            }
        }
    }
}
