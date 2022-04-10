using System;

namespace BeeBreeder.Common.Model.Environment
{
    public readonly struct Climate
    {
        public readonly Temperature Temperature;
        public readonly Humidity Humidity;

        public Climate(Temperature temperature, Humidity humidity)
        {
            Temperature = temperature;
            Humidity = humidity;
        }

        public static bool operator== (Climate first, Climate second)
        {
            return first.Humidity == second.Humidity && first.Temperature == second.Temperature;
        }
        
        public static bool operator!= (Climate first, Climate second)
        {
            return !(first == second);
        }

        public bool Equals(Climate other)
        {
            return Temperature == other.Temperature && Humidity == other.Humidity;
        }

        public override bool Equals(object obj)
        {
            return obj is Climate other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)Temperature, (int)Humidity);
        }
    }
}