using BeeBreeder.Management.Identifiers;
using System.Text;

namespace BeeBreeder.WebAPI.Sockets.Identifiers
{
    public class SimpleIdentifierGenerator : IIdentifierGenerator
    {
        private Random rand = new Random();
        private static char[] _allCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890".ToCharArray();
        private const int lenght = 8;
        public string GenerateIdentifier()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < lenght; i++)
            {
                sb.Append(_allCharacters[rand.Next(0, _allCharacters.Length)]);
            }
            return sb.ToString();
        }
    }
}
