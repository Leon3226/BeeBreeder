namespace BeeBreeder.Management.Model
{
    public class MoveRequest
    {
        public int FirstSide { get; set; }
        public int FirstSlot { get; set; }
        public int SecondSide { get; set; }
        public int SecondSlot { get; set; }
        public int Amount { get; set; }
    }
}
