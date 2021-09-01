using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Common.Model.Bees
{
    public interface IGenetical
    {
        Genotype Genotype { get; set; }
    }
}