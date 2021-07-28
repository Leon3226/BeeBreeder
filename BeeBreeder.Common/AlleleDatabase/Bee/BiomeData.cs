namespace BeeBreeder.Common.AlleleDatabase.Bee
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
    }
}