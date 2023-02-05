namespace BeeBreeder.Common.Model.Genetics.Phenotype
{
    public readonly struct Adaptation
    {
        public readonly int Up;
        public readonly int Down;
        public int Summary => Up + Down;

        public Adaptation(int up = 0, int down = 0)
        {
            Up = up;
            Down = down;
        }

        public Adaptation(string dataString)
        {
            if (dataString.StartsWith("+-")) 
            {
                var value = int.Parse(dataString.Replace("+-", ""));
                Up = Down = value;
                return;
            }
            if (dataString.StartsWith("+"))
            {
                var value = int.Parse(dataString.Replace("+", ""));
                Up = value;
                Down = 0;
                return;
            }
            if (dataString.StartsWith("-"))
            {
                var value = int.Parse(dataString.Replace("-", ""));
                Up = 0;
                Down = value;
                return;
            }

            Up = 0;
            Down = 0;
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