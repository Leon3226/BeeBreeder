namespace BeeBreeder.Breeding.Evaluation.Chance
{
    public static class BeeChangeChanceModel
    {
        //TODO: Revision
        //public static Dictionary<string, ChangeChance> GetChances(Bee bee, BeeCrossChance crossChance)
        //{
        //    var chances = new Dictionary<string, ChangeChance>();
        //    foreach (var chance in crossChance.Chances)
        //    {
        //        var newChance = new ChangeChance();
        //        var property = chance.First.Property;
        //        var beeChromosome = bee[property];
        //        foreach (var chromosomeChances in chance.Chances)
        //        {
        //            var comparison = chromosomeChances.Value.ParetoBetter(beeChromosome);
        //            if (comparison == null)
        //                newChance.ChanceToStay += chromosomeChances.Probability;
        //            if (comparison == beeChromosome)
        //                newChance.ChanceToSpoil += chromosomeChances.Probability;
        //            if (comparison == chromosomeChances.Value)
        //                newChance.ChanceToImprove += chromosomeChances.Probability;
        //        }
        //        chances.Add(property, newChance);
        //    }

        //    return chances;
        //}
    }
}