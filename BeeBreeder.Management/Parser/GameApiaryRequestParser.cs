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

        public Inventory[] ToInventories(string raw)
        {
            var inventories = new Inventory[6];
            var parsed = JObject.Parse(raw);
            parsed.Children().ToList().ForEach(x =>
            {
                var index = int.Parse(x.Path);
                var token = x.Last.SelectToken("");
                if (token.HasValues)
                {
                    var value = token.ToObject<Inventory>();
                    inventories[index] = value;
                }
            });
            return inventories;
        }

        public Item[] ToItems(string raw)
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
                        var beeItem = x.ToObject<BeeItem>();
                        beeItem.BeeData = x.ToObject<GameBeeModel>().ToModelBee();
                        item = beeItem;
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
