using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeBreeder.Property.Misc
{
    public class CodeGenerator
    {
        private Random rand = new Random();
        private static char[] _allCharacters = "1234567890".ToCharArray();
        private const int lenght = 5;
        public string GenerateCode()
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
