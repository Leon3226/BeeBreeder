using BeeBreeder.Property.Misc;
using BeeBreeder.Property.Model;
using BeeBreeder.Property.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Context = BeeBreeder.Data.Models.BeeBreederContext;

namespace BeeBreeder.Data.Repositories
{
    public class ComputerBindRequestRepository : IComputerBindRequestRepository
    {
        private readonly CodeGenerator _codeGenerator;

        public ComputerBindRequestRepository(CodeGenerator codeGenerator)
        {
            _codeGenerator = codeGenerator;
        }
        public async Task<bool> AssertAsync(string computerId, string userId, string code)
        {
            //var lastRequest = await LastRequestAsync(computerId, userId);
            //using (var context = new Context())
            //{
            //    var dbRequest = context
            //    await context.ComputerBindRequests.AddAsync(new Models.ComputerBindRequest()
            //    {
            //        Code = _codeGenerator.GenerateCode(),
            //        ComputerId = computerBindRequest.ComputerIdentifier,
            //        Created = DateTime.UtcNow,
            //        Failed = false,
            //        Resolved = false,
            //        TimeValid = timeValid,
            //        UserId = userId
            //    });

            //    await context.SaveChangesAsync();
            //    return code;
            //}
            throw new NotImplementedException();
        }

        public async Task<string> CreateRequestAsync(ComputerBindRequest computerBindRequest, string userId, TimeSpan timeValid)
        {
            using (var context = new Context())
            {
                var code = _codeGenerator.GenerateCode();
                await context.ComputerBindRequests.AddAsync(new Models.ComputerBindRequest()
                {
                    Code = _codeGenerator.GenerateCode(),
                    ComputerId = computerBindRequest.ComputerIdentifier,
                    Created = DateTime.UtcNow,
                    Failed = false,
                    Resolved = false,
                    TimeValid = timeValid,
                    UserId = userId
                });

                await context.SaveChangesAsync();
                return code;
            }
        }

        public async Task<ComputerBindRequest> LastRequestAsync(string computerId, string userId)
        {
            using (var context = new Context())
            {
                var lastRequest = context.ComputerBindRequests.Where(x =>
                x.ComputerId == computerId &&
                x.UserId == userId).Select(x => new ComputerBindRequest()
                {
                    ComputerIdentifier = x.ComputerId,
                    ConfirmCode = x.Code,
                    Created = x.Created,
                    Failed = x.Failed,
                    Resolved = x.Resolved,
                    TimeValid = x.TimeValid
                }).SingleOrDefault();

                return lastRequest;
            }
        }
    }
}
