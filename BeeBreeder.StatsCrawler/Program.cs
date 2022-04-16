// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using System.Text.RegularExpressions;
using BeeBreeder.StatsCrawler;
using HtmlAgilityPack;


string text = File.ReadAllText(@"C:\beeParseData.json");

var data  = JsonSerializer.Deserialize<List<BeeParsingData>>(text);

Console.ReadKey();

    