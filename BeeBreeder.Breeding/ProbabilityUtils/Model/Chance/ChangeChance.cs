namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Chance
{
    public struct ChangeChance
    {
        public double ChanceToSpoil;
        public double ChanceToImprove;
        public double ChanceToStay;

        public override string ToString()
        {
            return $@"spoil ({ChanceToSpoil*100:0.00}%) stay ({ChanceToStay*100:0.00}%) improve ({ChanceToImprove*100:0.00}%)";
        }
    }
}