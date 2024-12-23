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

        public Fish(string type, int maxTemperature, int maxTime, int minTemperature, int minTime)
        {
            Type = type;
            MaxTemperature = maxTemperature;
            MaxTime = maxTime;
            MinTemperature = minTemperature;
            MinTime = minTime;
        }
    }
}
