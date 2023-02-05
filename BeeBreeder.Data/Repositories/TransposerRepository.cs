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
    public class TransposerRepository : ITransposerRepository
    {
        public async Task AddTransposerAsync(int computerId, Transposer transposer)
        {
            using (var context = new Context())
            {
                var flowers = context.Flowers.Where(x => transposer.Flowers.Contains(x.Name));

                await context.AddAsync(new Models.TransposerDatum
                {
                    Id = transposer.Id,
                    ComputerId = computerId,
                    Biome = transposer.Biome,
                    Description = transposer.Description,
                    Name = transposer.Name,
                    Roofed = transposer.Roofed,
                    TransposerFlowers = flowers.Select(x => new Models.TransposerFlower { Flower = x}).ToArray()
                });
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteTransposerAsync(string id)
        {
            using (var context = new Context())
            {
                var dbTransposer = context.TransposerData.Single(x => x.Id == id);
                if (dbTransposer != null)
                {
                    context.TransposerData.Remove(dbTransposer);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<Transposer> GetTransposerAsync(string id)
        {
            using (var context = new Context())
            {
                return await Task.Run(() =>
                {
                    return context.TransposerData.Where(x => x.Id == id).Select(x => new Transposer()
                    {
                        Biome = x.Biome,
                        Name = x.Name,
                        Roofed = x.Roofed ?? false,
                        Description = x.Description,
                        Id = x.Id,
                        Flowers = x.TransposerFlowers.Select(x => x.Flower.Name).ToArray()
                    }).SingleOrDefault();
                });
            }
        }

        public async Task<Transposer[]> GetTransposersAsync(int computerId)
        {
            using (var context = new Context())
            {
                return await Task.Run(() =>
                {
                    return context.TransposerData.Where(x => x.ComputerId == computerId).Select(x => new Transposer()
                    {
                        Biome = x.Biome,
                        Name = x.Name,
                        Roofed = x.Roofed ?? false,
                        Description = x.Description,
                        Id = x.Id,
                        Flowers = x.TransposerFlowers.Select(x => x.Flower.Name).ToArray()
                    }).ToArray();
                });
            }
        }

        public async Task UpdateTransposerAsync(Transposer transposer, string id, int computerId)
        {
            using (var context = new Context())
            {
                var flowers = context.Flowers.Where(x => transposer.Flowers.Contains(x.Name));

                var dbTransposer = context.TransposerData.SingleOrDefault(x => x.Id == id);
                if (dbTransposer != null)
                {
                    dbTransposer.ComputerId = computerId;
                    dbTransposer.Biome = transposer.Biome;
                    dbTransposer.Description = transposer.Description;
                    dbTransposer.Name = transposer.Name;
                    dbTransposer.Roofed = transposer.Roofed;
                    context.TransposerFlowers.RemoveRange(context.TransposerFlowers.Where(x => x.TransposerId == id).ToList());
                    dbTransposer.TransposerFlowers = flowers.Select(x => new Models.TransposerFlower { Flower = x }).ToList();
                }
                await context.SaveChangesAsync();
            }
        }


    }
}
