using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeBreeder.Common.Data.Model;
using BeeBreeder.Common.Model.Genetics;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Common.Data
{
    public interface ISpecieCombinationsRepository
    {
        List<SpecieCombination> SpecieCombinations { get; }
        IEnumerable<SpecieCombination> GetPossibleMutations(Species first, Species second);
        public (Species?, Species?) Mutations(Chromosome<Species> first, Chromosome<Species> second);
    }
}
