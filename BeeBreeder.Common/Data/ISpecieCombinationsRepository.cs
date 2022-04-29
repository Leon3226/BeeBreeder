using System.Collections.Generic;
using BeeBreeder.Common.Data.Model;
using BeeBreeder.Common.Model.Genetics;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Common.Data
{
    public interface ISpecieCombinationsRepository
    {
        List<SpecieCombination> SpecieCombinations { get; }
        IEnumerable<SpecieCombination> GetPossibleMutations(string first, string second);
        public (string, string) Mutations(Chromosome<string> first, Chromosome<string> second);
    }
}
