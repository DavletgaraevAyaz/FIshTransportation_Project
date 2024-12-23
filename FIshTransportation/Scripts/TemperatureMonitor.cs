using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test22.Scripts
{
    public class TemperatureMonitor
    {
        private int minTemp;
        private int maxTemp;
        private int[] temperatures;
        private DateTime startTime;

        public TemperatureMonitor(DateTime startTime, int minTemp, int maxTemp, int[] temperatures)
        {
            this.startTime = startTime;
            this.minTemp = minTemp;
            this.maxTemp = maxTemp;
            this.temperatures = temperatures;
        }

        public string CheckConditions()
        {
            List<string> reportLines = new List<string>();
            TimeSpan totalMinViolation = TimeSpan.Zero;
            TimeSpan totalMaxViolation = TimeSpan.Zero;

            // Заголовок таблицы
            reportLines.Add("Время\tФакт\tНорма\tОтклонение от нормы");

            for (int i = 0; i < temperatures.Length; i++)
            {
                DateTime currentTime = startTime.AddMinutes(i * 10);
                int currentTemp = temperatures[i];

                if (currentTemp < minTemp)
                {
                    int deviation = minTemp - currentTemp;
                    reportLines.Add($"{currentTime:dd.MM.yyyy HH:mm}\t{currentTemp}\t{minTemp}\t{deviation}");
                    totalMinViolation += TimeSpan.FromMinutes(10);
                }
                else if (currentTemp > maxTemp)
                {
                    int deviation = currentTemp - maxTemp;
                    reportLines.Add($"{currentTime:dd.MM.yyyy HH:mm}\t{currentTemp}\t{maxTemp}\t{deviation}");
                    totalMaxViolation += TimeSpan.FromMinutes(10);
                }
            }

            string report = "Отчет\n";
            if (totalMinViolation.TotalMinutes > 0)
            {
                report += $"Порог минимально допустимой температуры превышен на {totalMinViolation.TotalMinutes} минут.\n\n";
            }
            if (totalMaxViolation.TotalMinutes > 0)
            {
                report += $"Порог максимальной допустимой температуры превышен на {totalMaxViolation.TotalMinutes} минут.\n\n";
            }

            report += string.Join("\n", reportLines);

            return report;
        }
    }
}