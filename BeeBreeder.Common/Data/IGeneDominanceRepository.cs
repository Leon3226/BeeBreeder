using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeBreeder.Common.Data
{
    public interface IGeneDominanceRepository
    {
        Dictionary<string, Dictionary<object, bool>> GenesDominance { get; }
    }
}
