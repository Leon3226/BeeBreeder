using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Strategy;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Data;
using BeeBreeder.Common.Model.Extensions;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Breeding.Targeter
{
    public class BestGenesTargeter : ISpecieTargeter
    {
        private readonly IStrategyUtils _strategyUtils;
        private readonly MutationTree _tree;
        public BestGenesTargeter(IStrategyUtils strategyUtils, MutationTree tree)
        {
            _strategyUtils = strategyUtils;
            _tree = tree;
        }

        public BeePool Bees { private get; set; }
        public IEnumerable<Species> ManualTargets { get; set; } = new List<Species>() {Species.Exotic};

        public IEnumerable<Species> CalculatedTargets
        {
            get
            {
                var strategy = _strategyUtils.ImportantTargets(Bees);
                List<Species> statNecessarySpecies = new List<Species>();
                statNecessarySpecies.AddRange(strategy.Species);
                statNecessarySpecies = statNecessarySpecies.Distinct().ToList();

                var target = ManualTargets.Concat(statNecessarySpecies).Distinct().ToList();
                var intermediateSpecies = _tree.OnlyNecessaryForGettingIfPossibleAndHaveEnough(target,
                    Bees.Bees.ExtractSpecies());

                var allTargets = intermediateSpecies.Concat(target).Distinct().ToList();
                return allTargets;
            }
        }

    }
}
