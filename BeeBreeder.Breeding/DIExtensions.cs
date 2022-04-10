using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeBreeder.Breeding.Analyzer;
using BeeBreeder.Breeding.Crossing;
using BeeBreeder.Breeding.EnvironmentMatching;
using BeeBreeder.Breeding.Flusher;
using BeeBreeder.Breeding.Generation;
using BeeBreeder.Breeding.Positioning;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Strategy;
using BeeBreeder.Breeding.Simulator;
using BeeBreeder.Breeding.Targeter;
using BeeBreeder.Common.Data;
using BeeBreeder.Common.Model.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BeeBreeder.Breeding
{
    public static class DIExtensions
    {
        public static IServiceCollection AddBeeBreeder(this IServiceCollection services)
        {
            services.AddScoped<IBreedAnalyzer, ExtendedNaturalSelectionAnalyzer>();
            services.AddScoped<IBreedFlusher, ExtendedNaturalSelectionFlusher>();
            services.AddScoped<MutationTree>(s => MutationTree.FromSpecieCombinations(s.GetRequiredService<ISpecieCombinationsRepository>().SpecieCombinations));
            services.AddScoped<IStrategyUtils, StrategyUtils>();
            services.AddScoped<IBreedingSimulator, BreedingSimulator>();
            services.AddScoped<ISpecieTargeter, BestGenesTargeter>();
            services.AddScoped<IPositionsController, PositionsController>();
            services.AddScoped<IBeeCrosser, BeeCrosser>();
            services.AddScoped<IEnvironmentMatcher, EnvironmentMatcher>();
            services.AddScoped<BeeGenerator>();

            var repo = new StubBeeDataRepository();
            services.AddSingleton<IGeneDominanceRepository>(repo);
            services.AddSingleton<ISpecieCombinationsRepository>(repo);
            services.AddSingleton<ISpecieStatsRepository>(repo);
            services.AddSingleton<IBiomeInfoRepository>(repo);
            services.AddSingleton<ISpecieClimateRepository>(repo);
            services.AddSingleton<IBiomeClimateRepository>(repo);

            return services;
        }
    }
}
