namespace BeeBreeder.Breeding.Evaluation.Chance
{
    public class Chance<T>
    {
        public double Probability;
        public T Value;

        public override string ToString()
        {
            return $"{Probability*100:0.00}% {Value}";
        }
    }
}