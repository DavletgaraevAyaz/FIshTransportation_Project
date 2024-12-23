using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIshTransportation.Scripts
{
    public class TemperatureRecord
    {
        public DateTime DateTime { get; set; }
        public int Temperature { get; set; }

        public TemperatureRecord(DateTime dateTime, int temperature)
        {
            DateTime = dateTime;
            Temperature = temperature;
        }
    }
}
