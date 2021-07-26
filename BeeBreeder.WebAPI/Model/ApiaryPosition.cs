using System;

namespace BeeBreeder.WebAPI.Model
{
    public struct ApiaryPosition
    {
        public Guid Trans;
        public int Side;
        public int Slot;

        public override string ToString()
        {
            return $"{Trans} - {Side}:{Slot}";
        }
    }
}