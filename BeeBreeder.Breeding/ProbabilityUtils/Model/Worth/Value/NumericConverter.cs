using System;
using System.Collections.Generic;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Value
{
    public class NumericConverter : INumericConverter
    {
        private static readonly Dictionary<Type, Func<IGene, double>> Comparers = new ();

        public NumericConverter()
        {
            Comparers.Add(typeof(Gene<int>), ConvertInt);
            Comparers.Add(typeof(Gene<Species>), ConvertSpecie);
            //Comparers.Add(typeof(Gene<Flowers>), ConvertFlowers);
            //Comparers.Add(typeof(Gene<Adaptation>), ConvertAdaptation);
            //Comparers.Add(typeof(Gene<Effect>), ConvertEffect);
        }

        public double GetNumericValue(IGene gene)
        {
            throw new System.NotImplementedException();
        }

        private static double ConvertInt(IGene gene)
        {
            return Convert.ToDouble(gene.Value);
        }
        
        private static double ConvertSpecie(IGene gene)
        {
            var converted = (Gene<Species>)gene;
            return 0;
        }
    }
}