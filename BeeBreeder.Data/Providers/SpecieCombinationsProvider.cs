using BeeBreeder.Common;
using BeeBreeder.Common.Data;
using BeeBreeder.Common.Data.Model;
using BeeBreeder.Common.Model.Genetics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Context = BeeBreeder.Data.Models.BeeBreederContext;

namespace BeeBreeder.Data.Providers
{
    public class SpecieCombinationsProvider : ISpecieCombinationsProvider
    {
        private List<SpecieCombination> _specieCombinationsCache;
        private List<int> avaliableMods = new List<int> { 4 };
        private bool _cached = false;
        public List<SpecieCombination> SpecieCombinations
        {
            get
            {
                if (!_cached)
                {
                    using (var context = new Context())
                    {
                        _specieCombinationsCache = context.MutationChancesHiveds
                            .Where(x =>
                                avaliableMods.Contains(x.ResultModId.Value) &&
                                avaliableMods.Contains(x.FirstModId.Value) &&
                                avaliableMods.Contains(x.SecondModId.Value)) //TODO: This is temp solution until mod support will be added
                            .Select(x => new SpecieCombination(x.FirstName, x.SecondName, x.Chance, x.ResultName)).ToList();
                        _cached = true;
                    }
                }
                return _specieCombinationsCache;
            }
        }

        public IEnumerable<SpecieCombination> GetPossibleMutations(string first, string second)
        {
                return SpecieCombinations.Where(x =>
                (x.Parent1 == first && x.Parent2 == second) ||
                (x.Parent1 == second && x.Parent2 == first)
                ).ToList();
        }

        public (string, string) Mutations(Chromosome<string> first, Chromosome<string> second)
        {
            string GetMutation(string firstChromosome, string secondChromosome)
            {
                //TODO: Move random to another entity
                return GetPossibleMutations(firstChromosome, secondChromosome).FirstOrDefault(x => x.MutationChance > RandomGenerator.Double())?.MutationResult;
            }

            string GetRandomMutation()
            {
                return RandomGenerator.Double() > 0.5
                    ? GetMutation(first.Primary.Value, second.Secondary.Value)
                    : GetMutation(first.Secondary.Value, second.Primary.Value);
            }

            return (GetRandomMutation(), GetRandomMutation());
        }
    }
}
