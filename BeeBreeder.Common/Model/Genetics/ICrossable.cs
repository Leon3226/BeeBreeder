using System;

namespace BeeBreeder.Common.Model.Genetics
{
    public interface ICrossable
    {
        public string Property { get; set; }
        ICrossable Cross(ICrossable second, Random random = null);
    }
}