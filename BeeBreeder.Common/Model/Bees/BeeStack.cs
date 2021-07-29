namespace BeeBreeder.Common.Model.Bees
{
    public class BeeStack
    {
        public Bee Bee;
        public int Count;

        public BeeStack()
        {
        }

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