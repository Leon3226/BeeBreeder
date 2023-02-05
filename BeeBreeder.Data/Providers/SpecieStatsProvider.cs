using BeeBreeder.Common.Data;
using BeeBreeder.Common.Data.Model;
using BeeBreeder.Common.Model.Genetics.Phenotype;
using BeeBreeder.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Context = BeeBreeder.Data.Models.BeeBreederContext;

namespace BeeBreeder.Data.Providers
{
    public class SpecieStatsProvider : ISpecieStatsProvider
    {
        private Dictionary<string, BeeInitialStats> _specieStatsCache;
        private bool _cached = false;

        public Dictionary<string, BeeInitialStats> SpecieStats
        {
            get
            {
                if (!_cached)
                {
                    _specieStatsCache = LoadStats();
                    _cached = true;
                }
                return _specieStatsCache;
            }
        }

        private Dictionary<string, BeeInitialStats> LoadStats()
        {
            Dictionary<string, BeeInitialStats> toReturn = new();
            using (var context = new Context())
            {
                var species = context.SpecieFulls.ToList();
                species.ForEach(specie =>
                {
                    var stats = new BeeInitialStats()
                    {
                        Characteristics = new Dictionary<string, object>()
                            {
                                {Constants.StatNames.Specie, specie.Name},
                                {Constants.StatNames.Lifespan, specie.Lifespan},
                                {Constants.StatNames.Speed, specie.Speed},
                                {Constants.StatNames.Pollination, specie.Pollination},
                                {Constants.StatNames.Flowers, specie.Flowers},
                                {Constants.StatNames.Fertility, specie.Fertility},
                                {Constants.StatNames.Area, specie.Territory.CalculateVolume()},
                                {Constants.StatNames.TempTolerance, new Adaptation(specie.TempTolerance)},
                                {Constants.StatNames.HumidTolerance, new Adaptation(specie.HumidTolerance)},
                                {Constants.StatNames.Diurnal, 1},
                                {Constants.StatNames.Nocturnal, Convert.ToInt32(specie.Nocturnal)},
                                {Constants.StatNames.Flyer, Convert.ToInt32(specie.RainTolerant)},
                                {Constants.StatNames.Cave, Convert.ToInt32(specie.CaveDwelling)},
                                {Constants.StatNames.Effect, specie.Effect ?? "None"}
                            }
                    };
                    toReturn.Add(specie.Name, stats);
                });
            }
            return toReturn;
        }
    }
}
