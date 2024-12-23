using System;

namespace FIshTransportation.Scripts
{
    public class Fish
    {
        public string Type { get; set; }
        public int MaxTemperature { get; set; }
        public int MaxTime { get; set; }
        public int MinTemperature { get; set; }
        public int MinTime { get; set; }
    }

    public class TemperatureRecord
    {
        public string DateTime { get; set; }
        public int Temperature { get; set; }
    }

    public class Report
    {
        public DateTime ViolationTime { get; set; }
        public int RequiredTemperature { get; set; }
        public int ActualTemperature { get; set; }
        public int Deviation { get; set; }
    }
}
