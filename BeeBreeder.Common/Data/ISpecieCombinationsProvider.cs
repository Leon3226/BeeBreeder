using System.Collections.Generic;
using BeeBreeder.Common.Data.Model;
using BeeBreeder.Common.Model.Genetics;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Common.Data
{
    public interface ISpecieCombinationsProvider
    {
        List<SpecieCombination> SpecieCombinations { get; }
        IEnumerable<SpecieCombination> GetPossibleMutations(string first, string second);
        //TODO: This shouldnt be here
        public (string, string) Mutations(Chromosome<string> first, Chromosome<string> second);
    }
}
