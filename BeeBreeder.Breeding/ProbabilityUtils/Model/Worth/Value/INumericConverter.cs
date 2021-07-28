using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Value
{
    public interface INumericConverter
    {
        double GetNumericValue(IGene gene);
    }
}