using BeeBreeder.Property.Model;
using System.Threading.Tasks;

namespace BeeBreeder.Property.Repository
{
    public interface IApiaryRepository
    {
        Task<Apiary[]> GetApiariesAsync(string userId);
        Task<Apiary> GetApiaryAsync(string userId, int id);
        Task AddApiaryAsync(string userId, Apiary apiary);
        Task UpdateApiaryAsync(Apiary apiary);
        Task DeleteApiaryAsync(int id);
    }
}
