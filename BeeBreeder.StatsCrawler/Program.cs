// See https://aka.ms/new-console-template for more information

using System.Collections;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.RegularExpressions;
using BeeBreeder.Common.Data;
using BeeBreeder.Common.Model.Environment;
using BeeBreeder.Data.Models;
using BeeBreeder.StatsCrawler;
using HtmlAgilityPack;

var connection1 = new BeeBreederContext();
var items = connection1.Items;
string text = File.ReadAllText(@"F:\beeParseData.json");

var data = JsonSerializer.Deserialize<List<BeeParsingData>>(text);
var mutations = data.Where(x => x.Mod == "Forestry").SelectMany(x => x.Mutations).ToArray();

using (var connection = new BeeBreederContext())
{
    //var mods = data.Select(x => x.Mod).Distinct().Select(x => new Mod() { Name = x });
    //var loot = data.SelectMany(x => x.Products).Concat(data.SelectMany(x => x.SpecialtyProducts)).Select(x => x.Product).Distinct().ToList();
    //
    //connection.Mods.AddRange(mods);
    //connection.Items.AddRange(loot.Select(x => new Item() {Name = x}));
    //
    //connection.SaveChanges();

    var fulls = connection.SpecieFulls.ToList();
    foreach (var speciestat in fulls)
    {
        var effect = data.FirstOrDefault(x => x.Name.Replace("Bee", "").Trim() == speciestat.Name).Effect;
        var stat = connection.SpecieStats.SingleOrDefault(x => x.SpecieId == speciestat.Id);
        stat.Effect = effect;
    }

    foreach (var beeData in data)
    {
        //var all = connection.Species.Where(x => beeData.Name.Contains(x.Name));
        //var dbBee = connection.Species.SingleOrDefault(x => beeData.Name.Replace("Bee", "").Trim() == x.Name);
        //
        //AddProducts(beeData.Products, false);
        //AddProducts(beeData.SpecialtyProducts, true);

        //void AddProducts(IEnumerable<ProductChance> products, bool isSpecialization)
        //{
        //    foreach (var product in products)
        //    {
        //        var dbProduct = connection.Items.SingleOrDefault(x => product.Product == x.Name);
        //        if (Double.TryParse(product.Chance.Replace("Base Chance:", "").Replace("%", "").Trim(), out double chanceValue))
        //        {
        //            var chance = new ItemDropChance
        //            {
        //                ItemId = dbProduct.Id,
        //                SpecieId = dbBee.Id,
        //                Chance = chanceValue * 0.01,
        //                IsSpecialization = isSpecialization
        //            };
        //            connection.ItemDropChances.Add(chance);
        //        }
        //        else
        //        {
        //            throw new Exception();
        //        }
        //    }
        //} 

        //var chances = beeData.Mutations.Select(x =>
        //{
        //    var firstName = x.First?.Replace("Bee", "").Trim();
        //    var secondName = x.Second?.Replace("Bee", "").Trim();
        //    var resultName = x.Result?.Replace("Bee", "").Trim();
        //    if (firstName == null || secondName == null || resultName == null || !Double.TryParse(x.Chance.Replace("%", "").Trim(), out double chance))
        //        return null;
        //
        //    var firstId = connection.Species.FirstOrDefault(specie => x.First != null && x.First.Replace("Bee", "").Trim() == specie.Name)?.Id;
        //    var secondId = connection.Species.FirstOrDefault(specie => x.Second != null && x.Second.Replace("Bee", "").Trim() == specie.Name)?.Id;
        //    var resultId = connection.Species.FirstOrDefault(specie => x.Result != null && x.Result.Replace("Bee", "").Trim() == specie.Name)?.Id;
        //
        //    if (firstName == "Hive" || secondName == "Hive")
        //    {
        //        Console.WriteLine();
        //    }
        //    if (!resultId.HasValue)
        //        return null;
        //
        //    if (!firstId.HasValue && firstName != "Hive")
        //        return null;
        //
        //    if (!secondId.HasValue && secondName != "Hive")
        //        return null;
        //
        //    return new MutationChance()
        //    {
        //        FirstId = firstId,
        //        SecondId = secondId,
        //        ResultId = resultId.Value,
        //        Chance = chance / 100
        //    };
        //
        //}).Where(x => x != null).ToList();
        //
        //connection.MutationChances.AddRange(chances);

        //var bee = new Specie()
        //{
        //    Name = beeData.Name.Replace("bee", "").Replace("Bee", "").Trim(),
        //    LatinName = beeData.LatinName.ToLower(),
        //    DiscoveredBy = beeData.DiscoveredBy,
        //    Description = beeData.Description,
        //    Branch = beeData.Branch.Replace("Branch", "").Trim(),
        //    ModId = connection.Mods.First(x => x.Name == beeData.Mod).Id
        //};
        //
        //connection.Species.Add(bee);
        //
        //connection.SaveChanges();
        //
        //var notes = beeData.Notes.Select(x => new SpecieNote() { Text = x, SpecieId = bee.Id });
        //
        //connection.SpecieNotes.AddRange(notes);
        //
        //connection.SaveChanges();
        //
        //var stats = new SpecieStat()
        //{
        //    Speed = Constants.SpeedStatNames.First(x => x.Name == beeData.Speed.ToLower()).Index,
        //    Lifespan = Constants.LifespanStatNames.First(x => x.Name == beeData.Lifespan.ToLower()).Index,
        //    Fertility = Int32.Parse(beeData.Fertility.Replace("Drones", "").Trim()),
        //    Pollination = Constants.PollinationStatNames.First(x => x.Name == beeData.Pollination.ToLower()).Index,
        //    Territory = beeData.Territory,
        //    Flowers = beeData.Flowers,
        //    CaveDwelling = beeData.CaveDwelling == "Yes",
        //    Nocturnal = beeData.Nocturnal == "Yes",
        //    RainTolerant = beeData.RainTolerant == "Yes",
        //    Temperature = (int)Enum.Parse<Temperature>(beeData.Temperature),
        //    Humidity = (int)Enum.Parse<Humidity>(beeData.Humidity),
        //    HumidTolerance = beeData.HumidTolerance == "None" ? "0" : beeData.HumidTolerance,
        //    TempTolerance = beeData.TempTolerance == "None" ? "0" : beeData.TempTolerance,
        //    SpecieId = bee.Id,
        //    IsDefault = true
        //};
        //
        //connection.SpecieStats.Add(stats);
        //connection.SaveChanges();
    }

    Console.WriteLine();
    connection.SaveChanges();

}


