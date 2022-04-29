using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Common.Model.Data
{
    public class MutationNode
    {
        public string Specie;
        public readonly List<MutationLink> Parents = new();
        public readonly List<MutationLink> Child = new();

        public bool LeadsTo(string specie)
        {
            return Child.Any(x => x.Child.Specie == specie || x.Child.LeadsTo(specie));
        }
        
        public bool LeadsToAny(IEnumerable<string> species)
        {
            return species.Any(LeadsTo);
        }

        public override string ToString()
        {
            return $"{Specie}";
        }
    }
}