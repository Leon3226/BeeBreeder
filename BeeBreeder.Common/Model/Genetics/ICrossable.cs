using System;

namespace BeeBreeder.Common.Model.Genetics
{
    public interface ICrossable
    {
        ICrossable Cross(ICrossable second, Random random = null);
    }
}