using System;
using BeeBreeder.Common.Model.Bees;

namespace BeeBreeder.Breeding.Breeder
{
    public class SimpleSpecieBreeder : IBeeBreeder
    {
        public BeePool Pool { get; set; }
        public void Breed(int iterations)
        {
            throw new NotImplementedException();
        }
    }
}