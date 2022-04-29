using System.Collections.Generic;
using System.Threading.Tasks;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.Comparison.Pareto
{
    public interface IParetoComparer
    {
        Bee ParetoBetter(Bee first, Bee second);
        IChromosome ParetoBetter(IChromosome first, IChromosome second);
        List<Bee> ParetoOptimal(IEnumerable<Bee> bees);
        Task<List<Bee>> ParetoOptimalAsync(IEnumerable<Bee> bees);
        List<BeeStack> ParetoOptimal(IEnumerable<BeeStack> bees);
        Task<List<BeeStack>> ParetoOptimalAsync(IEnumerable<BeeStack> bees);
    }
}
