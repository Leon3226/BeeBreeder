using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using BeeBreeder.Breeding.Analyzer;
using BeeBreeder.Breeding.Flusher;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Strategy;
using BeeBreeder.Breeding.Simulator;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Data;

namespace BeeBreeder
{
    internal static class DIConfig
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ExtendedNaturalSelectionAnalyzer>().As<IBreedAnalyzer>();
            builder.RegisterType<ExtendedNaturalSelectionFlusher>().As<IBreedFlusher>();
            builder.RegisterInstance<MutationTree>(MutationTree.FromSpecieCombinations(BeeGeneticDatabase.SpecieCombinations));
            builder.RegisterType<StrategyUtils>().As<IStrategyUtils>();
            builder.RegisterType<BreedingSimulator>().As<IBreedingSimulator>();

            return builder.Build();
        }
    }
}
