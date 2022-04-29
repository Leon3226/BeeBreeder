using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Breeding.Strategy;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Data;
using BeeBreeder.Common.Model.Extensions;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Breeding.Targeter
{
    public class BestGenesTargeter : ISpecieTargeter
    {
        private readonly IStrategySolver _strategyUtils;
        private readonly MutationTree _tree;
        public BestGenesTargeter(IStrategySolver strategyUtils, MutationTree tree)
        {
            _strategyUtils = strategyUtils;
            _tree = tree;
        }

        public BeePool Bees { private get; set; }
        public IEnumerable<string> ManualTargets { get; set; } = new List<string>() { "Exotic"};

        public IEnumerable<string> CalculatedTargets
        {
            get
            {
                var strategy = _strategyUtils.ImportantTargets(Bees);
                List<string> statNecessarySpecies = new List<string>();
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
