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
            if (Up == Down)
            {
                if (Up == 0 )
                {
                    return "0";
                }
                return $"+-{Up}";
            }

            var up = Up == 0 ? "" : $"+{Up}";
                var down = Down == 0 ? "" : $"-{Down}";
                return $"{up}{down}";
            }
        }
    }