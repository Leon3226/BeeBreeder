namespace BeeBreeder.Common.AlleleDatabase.Bee
{
    public struct Adaptation
    {
        public int Up;
        public int Down;

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