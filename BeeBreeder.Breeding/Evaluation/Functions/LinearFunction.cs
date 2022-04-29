namespace BeeBreeder.Breeding.Evaluation.Functions
{
    public class LinearFunction : IFunction
    {
        public double Coefficient = 1;
        public double FreeMember = 0;
        
        public double Y(double x)
        {
            return Coefficient * x + FreeMember;
        }
    }
}