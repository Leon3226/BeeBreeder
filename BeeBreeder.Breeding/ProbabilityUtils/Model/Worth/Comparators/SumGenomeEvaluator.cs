using System;
using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Comparators.Functions;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Genetics;
using StatNames = BeeBreeder.Common.AlleleDatabase.Bee.BeeGeneticDatabase.StatNames;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Comparators
{
    public class SumGenomeEvaluator : IGenomeEvaluator
    {
        public Dictionary<string, IFunction> GeneValueFunctions = new();

        public SumGenomeEvaluator()
        {
            GeneValueFunctions = new Dictionary<string, IFunction>()
            {
                {StatNames.Speed, new QuadraticFunction() {Coefficient = 0.7}},
                {StatNames.Area, new LinearFunction() {Coefficient = 2}},
                {StatNames.Cave, new LinearFunction() {Coefficient = 10}},
                {StatNames.Fertility, new QuadraticFunction() {Coefficient = 0.5}},
                {StatNames.Flyer, new LinearFunction() {Coefficient = 20}},
                {StatNames.Lifespan, new LinearFunction() {Coefficient = 1}},
                {StatNames.Nocturnal, new LinearFunction() {Coefficient = 25}},
                {StatNames.Pollination, new LinearFunction() {Coefficient = 3}},
            };
        }
        
        public double Evaluate(Genotype genotype)
        {
            return genotype.Genes.Sum(x =>
            {
                if (GeneValueFunctions.TryGetValue(x.Key, out IFunction func))
                {
                    if (double.TryParse(x.Value.Primary.Value.ToString(), out double val1) && double.TryParse(x.Value.Secondary.Value.ToString(), out double val2))
                        return func.Y(val1) + func.Y(val2);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            });
        }
    }
}