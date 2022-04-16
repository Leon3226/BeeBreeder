using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace BeeBreeder.StatsCrawler
{
    internal class Parser
    {
        public static List<BeeParsingData> GetAllBees()
        {
            string urlRegex = "https?:\\/\\/(?:www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b(?:[-a-zA-Z0-9()@:%_\\+.~#?&\\/=]*)";
            string url = "https://ftb.fandom.com";
            string mainPath = "/wiki/Forestry";
            HtmlWeb webDoc = new HtmlWeb();
            HtmlDocument doc = webDoc.Load(url + mainPath);
            var beeTableNode = doc.DocumentNode.SelectSingleNode("//*[@id='mw-content-text']//table[@class='navbox' and .//a[@href='/wiki/Category:Bees']]");
            var beeNodes = beeTableNode.SelectNodes(".//td[@class='navbox-list navbox-odd'][1]/div[1]/span[@class='nowrap navbox-item']/a");
            List<BeeParsingData> data = new List<BeeParsingData>();

            foreach (var beeNode in beeNodes)
            {
                Console.WriteLine(beeNode.InnerText);
                var beeLink = beeNode.Attributes["href"].Value;
                var beePage = webDoc.Load(url + beeLink);
                var pageData = beePage.DocumentNode.SelectSingleNode("//*[@id='mw-content-text']");
                var infoTable = pageData.SelectSingleNode(".//table[1]");
                var productionRows = infoTable.SelectNodes("tbody/tr[position()>=7]");
                var attributeTable = pageData.SelectSingleNode(".//table[2]");
                var mutationRows = pageData.SelectNodes(".//table[3]/tbody/tr[2]/td[2]/table/tbody/tr");
                var specialNotes = pageData.SelectNodes(".//ul/li");
                var beeParsingData = new BeeParsingData
                {
                    Name = beeNode.InnerText,
                    WikiUrl = beeLink,
                    ImageUrl = beeNode.ChildNodes[0].Attributes["style"].Value,
                    LatinName = infoTable.SelectSingleNode("tbody/tr[1]/td[2]/i")?.InnerText,
                    DiscoveredBy = infoTable.SelectSingleNode("tbody/tr[2]/td[2]/i")?.InnerText,
                    Mod = infoTable.SelectSingleNode("tbody/tr[3]/td[2]/i/a")?.InnerText,
                    Branch = infoTable.SelectSingleNode("tbody/tr[4]/td[2]/i/a")?.InnerText,
                    Nocturnal = infoTable.SelectSingleNode("tbody/tr[5]/td[2]/i")?.InnerText,
                    Description = infoTable.SelectSingleNode("tbody/tr[2]/td[3]")?.InnerText,

                    Speed = attributeTable.SelectSingleNode("tbody/tr[1]/td[2]/i")?.InnerText,
                    Lifespan = attributeTable.SelectSingleNode("tbody/tr[2]/td[2]/i")?.InnerText,
                    Fertility = attributeTable.SelectSingleNode("tbody/tr[3]/td[2]/i")?.InnerText,
                    Pollination = attributeTable.SelectSingleNode("tbody/tr[4]/td[2]/i")?.InnerText,
                    Territory = attributeTable.SelectSingleNode("tbody/tr[5]/td[2]/i")?.InnerText,
                    Flowers = attributeTable.SelectSingleNode("tbody/tr[6]/td[2]/i")?.InnerText,
                    Effect = attributeTable.SelectSingleNode("tbody/tr[7]/td[2]/i")?.InnerText,
                    CaveDwelling = attributeTable.SelectSingleNode("tbody/tr[1]/td[4]/i")?.InnerText,
                    RainTolerant = attributeTable.SelectSingleNode("tbody/tr[3]/td[4]/i")?.InnerText,
                    Temperature = attributeTable.SelectSingleNode("tbody/tr[4]/td[4]/i")?.InnerText,
                    TempTolerance = attributeTable.SelectSingleNode("tbody/tr[5]/td[4]/i")?.InnerText,
                    Humidity = attributeTable.SelectSingleNode("tbody/tr[6]/td[4]/i")?.InnerText,
                    HumidTolerance = attributeTable.SelectSingleNode("tbody/tr[7]/td[4]/i")?.InnerText
                };

                mutationRows?.Skip(1).ToList().ForEach(x => beeParsingData.Mutations.Add(new Mutation
                {
                    First = x.SelectSingleNode("td[1]/strong")?.InnerText,
                    Second = x.SelectSingleNode("td[2]/a")?.InnerText,
                    Result = x.SelectSingleNode("td[3]/a")?.InnerText,
                    Chance = x.SelectSingleNode("td[4]")?.InnerText
                }));


                specialNotes?.ToList().ForEach(x => beeParsingData.Notes.Add(x.InnerText));

                productionRows.ToList().ForEach(x =>
                {
                    var product = new ProductChance()
                    {
                        Product = x.SelectSingleNode("td[1]/a")?.InnerText,
                        Chance = x.SelectSingleNode("td[2]")?.InnerText
                    };
                    if (product.Product != null && product.Chance != null)
                    {
                        beeParsingData.Products?.Add(product);
                    }
                    var specialty = new ProductChance()
                    {
                        Product = x.SelectSingleNode("td[3]/a")?.InnerText,
                        Chance = x.SelectSingleNode("td[4]")?.InnerText
                    };
                    if (specialty.Product != null && specialty.Chance != null)
                    {
                        beeParsingData.SpecialtyProducts?.Add(specialty);
                    }
                });

                data.Add(beeParsingData);

            }

            return data;

        }
        
    }

}
