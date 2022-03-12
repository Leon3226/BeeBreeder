using BeeBreeder.Common.Model.Bees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeBreeder.Breeding.Simulator
{
    public interface IBreedingSimulator
    {
        BeePool Pool { get; set; }
        void Breed(int iterations);
    }
}
