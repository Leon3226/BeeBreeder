using BeeBreeder.Management.Model;
using BeeBreeder.Management.Model.InGame;
using BeeBreeder.WebAPI.Mapping;
using Newtonsoft.Json.Linq;

namespace BeeBreeder.Management.Parser
{
    public class GameApiaryRequestParser : IGameApiaryRequestParser
    {
        public int ToInt(string raw)
        {
            return int.Parse(raw);
        }

        public GameInventory[] ToInventories(string raw)
        {
            try
            {
                var inventories = new GameInventory[6];
                var parsed = JObject.Parse(raw);
                parsed.Children().ToList().ForEach(x =>
                {
                    var index = int.Parse(x.Path);
                    var token = x.Last.SelectToken("");
                    if (token.HasValues)
                    {
                        var value = token.ToObject<GameInventory>();
                        inventories[index] = value;
                    }
                });
                return inventories;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public Item[] ToItems(string raw)
        {
            try
            {
                var parsed = JArray.Parse(raw);
                var items = new Item[parsed.Count];
                var child = parsed.Children().ToList();
                child.ForEach(x =>
                {
                    var index = int.Parse(x.Path.Replace("[", "").Replace("]", ""));
                    if (x.HasValues)
                    {
                        Item item;
                        var beeData = x.SelectToken("individual");
                        if (beeData != null)
                        {
                            try
                            {
                                var beeItem = x.ToObject<BeeItem>();
                                var beeObject = x.ToObject<GameBeeModel>();
                                if (!beeObject.Individual.IsAnalyzed)
                                {
                                    throw new Exception("Bee is not analyzed");
                                }
                                beeItem.BeeData = beeObject.ToModelBee();
                                item = beeItem;
                            }
                            catch (Exception e)
                            {
                                item = x.ToObject<Item>();
                            }
                        }
                        else
                        {
                            item = x.ToObject<Item>();
                        }

                        items[index] = item;
                    }
                    else
                    {
                        items[index] = null;
                    }
                });
                return items;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public string[] ToTransposers(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return new string[0];
            var parsed = JObject.Parse(raw);
            var transposers = parsed.Children().Select(x => x.Path).ToArray();
            return transposers;
        }
    }
}
