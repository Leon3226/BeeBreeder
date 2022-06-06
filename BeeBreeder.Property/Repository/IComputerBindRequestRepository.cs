using BeeBreeder.Property.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeBreeder.Property.Repository
{
    public interface IComputerBindRequestRepository
    {
        Task<string> CreateRequestAsync(ComputerBindRequest computerBindRequest, string userId, TimeSpan timeValid);
        Task<ComputerBindRequest> LastRequestAsync(string computerId, string userId);
        Task<bool> AssertAsync(string computerId, string userId, string code);

    }
}
