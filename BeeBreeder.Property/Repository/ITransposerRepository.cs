using BeeBreeder.Property.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeBreeder.Property.Repository
{
    public interface ITransposerRepository
    {
        Task<Transposer[]> GetTransposersAsync(int computerId);
        Task<Transposer> GetTransposerAsync(string id);
        Task AddTransposerAsync(int computerId, Transposer transposer);
        Task UpdateTransposerAsync(Transposer transposer, string id, int computerId);
        Task DeleteTransposerAsync(string id);
    }

}
