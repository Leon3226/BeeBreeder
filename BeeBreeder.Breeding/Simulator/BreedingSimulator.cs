using BeeBreeder.Breeding.Analyzer;
using BeeBreeder.Common.Model.Bees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeBreeder.Breeding.Crossing;
using BeeBreeder.Breeding.Flusher;

namespace BeeBreeder.Breeding.Simulator
{
    public class BreedingSimulator : IBreedingSimulator
    {
        private readonly IBreedAnalyzer _breedAnalyzer;
        private readonly IBreedFlusher _breedFlusher;
        private readonly IBeeCrosser _beeCrosser;
        public BeePool Pool { get; set; } = new ();

        public BreedingSimulator(IBreedAnalyzer breedAnalyzer, IBreedFlusher breedFlusher, IBeeCrosser beeCrosser)
        {
            _breedAnalyzer = breedAnalyzer;
            _breedFlusher = breedFlusher;
            _beeCrosser = beeCrosser;
        }

        public void Breed(int iterations)
        {
            if (iterations < 0) return;
            for (int i = 0; i < iterations;)
            {
                var pairs = _breedAnalyzer.GetBreedingPairs(Pool);
                if (pairs.Count == 0)
                    break;

                i += pairs.Count;

                pairs.ForEach(x =>
                {
                    Pool.Bees.AddRange(_beeCrosser.Cross(x.Princess, x.Drone).Select(bee => new BeeStack(bee, 1)));
                    Pool.RemoveBee(x.Item1);
                    Pool.RemoveBee(x.Item2);
                });
                Pool.CompactDuplicates();

                Pool.RemoveAll(_breedFlusher.ToFlush(Pool));
            }
        }
    }
}
