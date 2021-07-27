namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Comparators.Functions
{
    public class QuadraticFunction : IFunction
    {
        public double Coefficient = 1;
        public double FreeMember = 0;
        
        public double Y(double x)
        {
            return Coefficient * x * x + FreeMember;
        }
    }
}