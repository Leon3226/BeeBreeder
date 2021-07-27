namespace BeeBreeder.Common.AlleleDatabase.Bee
{
    public readonly struct Adaptation
    {
        public readonly int Up;
        public readonly int Down;

        public Adaptation(int up = 0, int down = 0)
        {
            Up = up;
            Down = down;
        }

        public override string ToString()
        {
            return $"-{Up}+{Down}";
        }
    }
}