namespace BeeBreeder.Common.Model.Bees
{
    public class BeeStack
    {
        public Bee Bee { get; set; }
        public int Count { get; set; }

        public BeeStack(Bee bee, int count)
        {
            Bee = bee;
            Count = count;
        }

        public override string ToString()
        {
            return $@"({Count}) {Bee}";
        }
    }
}