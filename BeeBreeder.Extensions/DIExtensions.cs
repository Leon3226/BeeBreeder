using System;
using BeeBreeder.Breeding.Analyzer;
using BeeBreeder.Breeding.Comparison.Gene.Comparators;
using BeeBreeder.Breeding.Comparison.Gene.Priority;
using BeeBreeder.Breeding.Comparison.Pareto;
using BeeBreeder.Breeding.Crossing;
using BeeBreeder.Breeding.EnvironmentMatching;
using BeeBreeder.Breeding.Flusher;
using BeeBreeder.Breeding.Generation;
using BeeBreeder.Breeding.Positioning;
using BeeBreeder.Breeding.Simulator;
using BeeBreeder.Breeding.Strategy;
using BeeBreeder.Breeding.Targeter;
using BeeBreeder.Common.Data;
using BeeBreeder.Common.Model.Data;
using BeeBreeder.Data.Repositories;
using BeeBreeder.Management.Identifiers;
using BeeBreeder.Management.Parser;
using BeeBreeder.Management.Repository;
using BeeBreeder.Management.Sockets;
using BeeBreeder.Property.Misc;
using BeeBreeder.Property.Repository;
using BeeBreeder.WebAPI.Sockets.Identifiers;
using Microsoft.Extensions.DependencyInjection;

namespace BeeBreeder.Extensions
{
    //TODO: Move to another project
    public static class DIExtensions
    {
        public static IServiceCollection AddBeeBreeder(this IServiceCollection services)
        {
            services.AddScoped<IBreedAnalyzer, ExtendedNaturalSelectionAnalyzer>();
            services.AddScoped<IBreedFlusher, ExtendedNaturalSelectionFlusher>();
            services.AddScoped(s => MutationTree.FromSpecieCombinations(s.GetRequiredService<ISpecieCombinationsRepository>().SpecieCombinations));
            services.AddScoped<IStrategySolver, StrategySolver>();
            services.AddScoped<IBreedingSimulator, BreedingSimulator>();
            services.AddScoped<ISpecieTargeter, BestGenesTargeter>();
            services.AddScoped<IPositionsController, PositionsController>();
            services.AddScoped<IBeeCrosser, BeeCrosser>();
            services.AddScoped<IParetoComparer, ParetoComparer>();
            services.AddScoped<IGeneComparator>(CreateGeneComparer);
            services.AddScoped<IEnvironmentMatcher, EnvironmentMatcher>();
            services.AddScoped<IApiaryRepository, ApiaryRepository>();
            services.AddScoped<CodeGenerator>();
            services.AddScoped<IComputerBindRequestRepository, ComputerBindRequestRepository>();
            services.AddScoped<IComputerRepository, ComputerRepository>();
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

        public static IServiceCollection AddSocketManaging(this IServiceCollection services, System.Net.IPEndPoint endPoint)
        {
            var socketManager = new SocketManager(endPoint);
            socketManager.BeginAccept();
            services.AddTransient<IIdentifierGenerator, SimpleIdentifierGenerator>();
            services.AddSingleton<SocketManager>(socketManager);
            //TODO: Refactor
            services.AddSingleton<ApiariesSocketsManager>(new ApiariesSocketsManager(socketManager, new SimpleIdentifierGenerator()));
            services.AddTransient<IGameApiaryRequestParser, GameApiaryRequestParser>();
            services.AddTransient<IGameApiariesDataRepository, GameApiariesDataRepository>();

            return services;
        }

        //TODO: Refactor
        private static IGeneComparator CreateGeneComparer(IServiceProvider provider)
        {
            return new GeneComparator
            {
                Comparators =
                {
                    {Constants.StatNames.Specie, new StringTargetComparator(new PriorityProvider() {Priorities =  Constants.DefaultSpeciePriorities })},
                    {Constants.StatNames.Flowers, new StringTargetComparator(new PriorityProvider() {Priorities =  Constants.DefaultFlowersPriorities })},
                    {Constants.StatNames.Effect, new StringTargetComparator(new PriorityProvider() {Priorities =  Constants.DefaultEffectPriorities })},

                    {Constants.StatNames.HumidTolerance, new AdaptationComparator() {PreferSumBetterMode = true}},
                    {Constants.StatNames.TempTolerance, new AdaptationComparator() {PreferSumBetterMode = true}},

                    {Constants.StatNames.Speed, new UniversalIntComparator() {IsMoreBetterMode = true}},
                    {Constants.StatNames.Fertility,new UniversalIntComparator() {IsMoreBetterMode = true}},
                    {Constants.StatNames.Lifespan, new UniversalIntComparator() {IsMoreBetterMode = true}},
                    {Constants.StatNames.Area, new UniversalIntComparator() {IsMoreBetterMode = true}},
                    {Constants.StatNames.Pollination,new UniversalIntComparator() {IsMoreBetterMode = true}},
                    {Constants.StatNames.Diurnal, new UniversalIntComparator() {IsMoreBetterMode = true}},
                    {Constants.StatNames.Nocturnal, new UniversalIntComparator() {IsMoreBetterMode = true}},
                    {Constants.StatNames.Cave, new UniversalIntComparator() {IsMoreBetterMode = true}},
                    {Constants.StatNames.Flyer, new UniversalIntComparator() {IsMoreBetterMode = true}},
                }
            };
        }

    }
}
