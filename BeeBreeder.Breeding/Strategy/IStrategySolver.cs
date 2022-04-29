using BeeBreeder.Common.Model.Bees;

namespace BeeBreeder.Breeding.Strategy
{
    public interface IStrategySolver
    {
        StrategyResult ImportantTargets(BeePool pool, int minimalCount = 5);
    }
}