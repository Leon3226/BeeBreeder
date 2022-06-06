using BeeBreeder.Property.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeBreeder.Property.Repository
{
    public interface IComputerRepository
    {
        Task<ApiaryComputer[]> GetComputersAsync(string userId);
        Task<ApiaryComputer> GetComputerAsync(string userId, int id);
        Task<ApiaryComputer> GetComputerAsync(string identifier);
        Task AddComputerAsync(string userId, ApiaryComputer computer);
        Task UpdateComputerAsync(ApiaryComputer computer);
        Task DeleteComputerAsync(int id);
        Task SetApiary(int computerId, int? apiaryId);
    }
}
