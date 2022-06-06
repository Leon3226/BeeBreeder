using System;

namespace BeeBreeder.Common.Model.Positioning
{
    public struct ApiaryPosition
    {
        public string Trans;
        public int Side;
        public int Slot;

        public override string ToString()
        {
            return $"{Trans} - {Side}:{Slot}";
        }
    }
}