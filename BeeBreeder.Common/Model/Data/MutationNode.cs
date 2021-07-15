using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Common.AlleleDatabase.Bee;

namespace BeeBreeder.Common.Model.Data
{
    public class MutationNode
    {
        public Species Specie;
        public List<MutationLink> Parents = new();
        public List<MutationLink> Child = new();

        public bool LeadsTo(Species specie)
        {
            return Child.Any(x => x.Child.Specie == specie || x.Child.LeadsTo(specie));
        }

        public override string ToString()
        {
            return $"{Specie}";
        }
    }
}