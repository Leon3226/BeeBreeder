using BeeBreeder.Common.Model.Bees;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Strategy
{
    public interface IStrategyUtils
    {
        StrategyResult ImportantTargets(BeePool pool, int minimalCount = 5);
    }
}