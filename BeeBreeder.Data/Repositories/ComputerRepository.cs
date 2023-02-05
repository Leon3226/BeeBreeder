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
    public class ComputerRepository : IComputerRepository
    {
        public async Task AddComputerAsync(string userId, ApiaryComputer computer)
        {
            using (var context = new Context())
            {
                await context.AddAsync(new Models.ApiaryComputer
                {
                    Description = computer.Description,
                    InGameIdentifier = computer.Identifier,
                    Name = computer.Name,
                    UserId = userId
                });
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteComputerAsync(int id)
        {
            using (var context = new Context())
            {
                var dbComputer = context.ApiaryComputers.Single(x => x.Id == id);
                if (dbComputer != null)
                {
                    context.ApiaryComputers.Remove(dbComputer);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<ApiaryComputer[]> GetComputersAsync(string userId)
        {
            using (var context = new Context())
            {
                return await Task.Run(() =>
                {
                    return context.ApiaryComputers.Where(x => x.UserId == userId).Select(x => new ApiaryComputer()
                    {
                        Id = x.Id,
                        Identifier = x.InGameIdentifier,
                        Description = x.Description,
                        Name = x.Name,
                        UserId = x.UserId,
                        ApiaryId = x.ApiaryId
                    }).ToArray();
                });
            }
        }

        public async Task<ApiaryComputer> GetComputerAsync(string userId, int id)
        {
            using (var context = new Context())
            {
                return await Task.Run(() =>
                {
                    return context.ApiaryComputers.Where(x => x.Id == id && x.UserId == userId).Select(x => new ApiaryComputer()
                    {
                        Id = x.Id,
                        Identifier = x.InGameIdentifier,
                        Description = x.Description,
                        Name = x.Name,
                        UserId = x.UserId,
                        ApiaryId = x.ApiaryId
                    }).SingleOrDefault();
                });
            }
        }

        public async Task<ApiaryComputer> GetComputerAsync(string identifier)
        {
            using (var context = new Context())
            {
                return await Task.Run(() =>
                {
                    return context.ApiaryComputers.Where(x => x.InGameIdentifier == identifier).Select(x => new ApiaryComputer()
                    {
                        Id = x.Id,
                        Identifier = x.InGameIdentifier,
                        Description = x.Description,
                        Name = x.Name,
                        UserId = x.UserId,
                        ApiaryId = x.ApiaryId
                    }).SingleOrDefault();
                });
            }
        }

        public async Task SetApiary(int computerId, int? apiaryId)
        {
            using (var context = new Context())
            {
                var dbComputer = context.ApiaryComputers.SingleOrDefault(x => x.Id == computerId);
                var apiary = context.Apiaries.SingleOrDefault(x => x.Id == apiaryId);
                if (dbComputer != null && apiary != null)
                {
                    dbComputer.ApiaryId = apiaryId;
                }
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateComputerAsync(ApiaryComputer computer)
        {
            using (var context = new Context())
            {
                var dbComputer = context.ApiaryComputers.SingleOrDefault(x => x.Id == computer.Id);
                if (dbComputer != null)
                {
                    dbComputer.Name = computer.Name;
                    dbComputer.Description = computer.Description;
                }
                await context.SaveChangesAsync();
            }
        }

        public async Task DetachApiary(int computerId)
        {
            using (var context = new Context())
            {
                var dbComputer = context.ApiaryComputers.SingleOrDefault(x => x.Id == computerId);
                if (dbComputer != null)
                {
                    dbComputer.ApiaryId = null;
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
