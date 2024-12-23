using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test22.Scripts
{
    public class TemperatureMonitor
    {
            private int transportTime;
        private int minTemp;
        private int maxTemp;
        private int[] temperatures;

        public TemperatureMonitor(int transportTime, int minTemp, int maxTemp, int[] temperatures)
        {
            this.transportTime = transportTime;
            this.minTemp = minTemp;
            this.maxTemp = maxTemp;
            this.temperatures = temperatures;
        }

        public string CheckConditions()
        {
            StringBuilder report = new StringBuilder();
            List<string> violations = new List<string>();

            for (int i = 0; i < temperatures.Length; i++)
            {
                if (temperatures[i] > maxTemp)
                {
                    violations.Add($"Нарушение: Температура {temperatures[i]}°C превышает максимальную ({maxTemp}°C) на минуте {i * 10}.");
                }
                else if (temperatures[i] < minTemp)
                {
                    violations.Add($"Нарушение: Температура {temperatures[i]}°C ниже минимальной ({minTemp}°C) на минуте {i * 10}.");
                }
            }

            if (violations.Count > 0)
            {
                report.AppendLine("Обнаружены нарушения условий хранения:");
                foreach (var violation in violations)
                {
                    report.AppendLine(violation);
                }
            }
            else
            {
                report.AppendLine("Условия хранения соблюдены.");
            }

            return report.ToString();
        }
    }
}
