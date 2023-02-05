using BeeBreeder.Management.Model;

namespace BeeBreeder.Management.Parser
{
    public interface IGameApiaryRequestParser
    {
        string[] ToTransposers(string raw);
        GameInventory[] ToInventories(string raw);
        Item[] ToItems(string raw);
        int ToInt(string raw);
    }
}
