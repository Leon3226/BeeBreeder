using System.Text.RegularExpressions;

namespace BeeBreeder.Common.Model.Genetics
{
    public struct Gene<T> : IGene<T>
    {
        public T Property { get; set; }
        public bool Dominant { get; set; }

        public override string ToString()
        {
            return Property.ToString();
        }
    }

    
}