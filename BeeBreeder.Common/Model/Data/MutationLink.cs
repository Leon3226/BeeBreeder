namespace BeeBreeder.Common.Model.Data
{
    public class MutationLink
    {
        public double MutationChance;
        public MutationNode Parent1;
        public MutationNode Parent2;
        public MutationNode Child;
        
        public override string ToString()
        {
            return $"{Parent1}+{Parent2} =({MutationChance*100}%)={Child}";
        }
    }
}