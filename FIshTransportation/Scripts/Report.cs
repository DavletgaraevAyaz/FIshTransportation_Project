using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIshTransportation.Scripts
{
    public class Report
    {
        public DateTime ViolationTime { get; set; }
        public int RequiredTemperature { get; set; }
        public int ActualTemperature { get; set; }
        public int Deviation { get; set; }

        public Report(DateTime violationTime, int requiredTemperature, int actualTemperature, int deviation)
        {
            ViolationTime = violationTime;
            RequiredTemperature = requiredTemperature;
            ActualTemperature = actualTemperature;
            Deviation = deviation;
        }
    }
}
