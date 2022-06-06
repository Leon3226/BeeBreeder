using BeeBreeder.Data.Models;
using BeeBreeder.Property.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Context = BeeBreeder.Data.Models.BeeBreederContext;

namespace BeeBreeder.Data.Repositories
{
    public class ApiaryRepository : IApiaryRepository
    {
        public ApiaryRepository()
        {
            
        }

        public async Task AddApiaryAsync(string userId, Property.Model.Apiary apiary)
        {
            using (var context = new Context())
            {
                await context.AddAsync(new Apiary
                {
                    Name = apiary.Name,
                    Description = apiary.Description,
                    UserId = userId
                });
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteApiaryAsync(int id)
        {
            using (var context = new Context())
            {
                var apiary = context.Apiaries.SingleOrDefault(x => x.Id == id);
                if (apiary != null)
                {
                    context.Apiaries.Remove(apiary);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<Property.Model.Apiary[]> GetApiariesAsync(string userId)
        {
            using (var context = new Context())
            {
                return await Task.Run(() =>
                {
                    return context.Apiaries.Where(x => x.UserId == userId).Select(x => new Property.Model.Apiary()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Computers = x.ApiaryComputers.Select(x => x.InGameIdentifier).ToArray()
                    }).ToArray();
                });
            }
        }

        public async Task<Property.Model.Apiary> GetApiaryAsync(string userId, int id)
        {
            using (var context = new Context())
            {
                return await Task.Run(() =>
                {
                    return context.Apiaries.Where(x => x.Id == id && x.UserId == userId).Select(x => new Property.Model.Apiary()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Computers = x.ApiaryComputers.Select(x => x.InGameIdentifier).ToArray()
                    }).SingleOrDefault();
                });
            }
        }

        public async Task UpdateApiaryAsync(Property.Model.Apiary apiary)
        {
            using (var context = new Context())
            {
                var dbApiary = context.Apiaries.SingleOrDefault(x => x.Id == apiary.Id);
                if (dbApiary != null)
                {
                    dbApiary.Name = apiary.Name;
                    dbApiary.Description = apiary.Description;
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
