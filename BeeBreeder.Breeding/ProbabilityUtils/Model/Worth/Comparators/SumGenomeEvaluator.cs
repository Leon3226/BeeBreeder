using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Comparators.Functions;
using BeeBreeder.Common.Model.Genetics;
using StatNames = BeeBreeder.Common.Data.Constants.StatNames;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Comparators
{
    public class SumGenomeEvaluator : IGenomeEvaluator
    {
        public Dictionary<string, IFunction> GeneValueFunctions;

        public SumGenomeEvaluator()
        {
            GeneValueFunctions = new Dictionary<string, IFunction>()
            {
                {StatNames.Speed, new QuadraticFunction() {Coefficient = 2}},
                {StatNames.Area, new LinearFunction() {Coefficient = 2}},
                {StatNames.Cave, new LinearFunction() {Coefficient = 10}},
                {StatNames.Fertility, new QuadraticFunction() {Coefficient = 0.5}},
                {StatNames.Flyer, new LinearFunction() {Coefficient = 30}},
                {StatNames.Lifespan, new LinearFunction() {Coefficient = 1}},
                {StatNames.Nocturnal, new LinearFunction() {Coefficient = 50}},
                {StatNames.Pollination, new LinearFunction() {Coefficient = 3}},
                {StatNames.HumidTolerance, new LinearFunction() {Coefficient = 5}},
                {StatNames.TempTolerance, new LinearFunction() {Coefficient = 5}},
                {StatNames.Flowers, new LinearFunction() {Coefficient = 3}},
                {StatNames.Effect, new LinearFunction() {Coefficient = 10}}
            };
        }

        public double Evaluate(Genotype genotype)
        {
            return genotype.Chromosomes.Sum(x =>
            {
                if (GeneValueFunctions.TryGetValue(x.Key, out IFunction func))
                    return func.Y(x.Value.Primary.Numeric()) + func.Y(x.Value.Secondary.Numeric());
                return 0;
            });
        }
    }
}