using System.Collections.Generic;
using System.Threading.Tasks;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.Comparison.Pareto
{
    public interface IParetoComparer
    {
        Bee ParetoBetter(Bee first, Bee second, BreedingTarget target = null);
        IChromosome ParetoBetter(IChromosome first, IChromosome second, bool compareOrder = false, BreedingTarget target = null);
        List<Bee> ParetoOptimal(IEnumerable<Bee> bees, BreedingTarget target = null);
        Task<List<Bee>> ParetoOptimalAsync(IEnumerable<Bee> bees, BreedingTarget target = null);
        List<BeeStack> ParetoOptimal(IEnumerable<BeeStack> bees, BreedingTarget target = null);
        Task<List<BeeStack>> ParetoOptimalAsync(IEnumerable<BeeStack> bees, BreedingTarget target = null);
    }
}
