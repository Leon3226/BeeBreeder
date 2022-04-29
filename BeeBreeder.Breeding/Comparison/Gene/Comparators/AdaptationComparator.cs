using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Breeding.Comparison.Gene.Comparators
{
    public class AdaptationComparator : IGeneValueComparator<Adaptation>
    {
        private bool _preferLowerMode;
        private bool _preferHigherMode;

        public bool PreferLowerMode
        {
            get => _preferLowerMode;
            set
            {
                ResetAllModes();
                _preferLowerMode = value;
            }
        }

        public bool PreferHigherMode
        {
            get => _preferHigherMode;
            set
            {
                ResetAllModes();
                _preferHigherMode = value;
            }
        }

        public bool PreferSumBetterMode
        {
            get => !PreferLowerMode && !PreferHigherMode;
            set => ResetAllModes();
        }

        private void ResetAllModes()
        {
            _preferLowerMode = false;
            _preferHigherMode = false;
        }

        public Comparison Compare(Adaptation first, Adaptation second)
        {
            if (first.Equals(second))
                return Comparison.Equal;

            if (PreferHigherMode && first.Up != second.Up)
            {
                return first.Up > second.Up
                    ? Comparison.Better
                    : Comparison.Worse;
            }

            if (PreferHigherMode && first.Down != second.Down)
            {
                return first.Down > second.Down
                    ? Comparison.Better
                    : Comparison.Worse;
            }
             
            return first.Summary > second.Summary
                ? Comparison.Better
                : Comparison.Worse;
        }

        public Comparison Compare(object first, object second)
        {
            return Compare((Adaptation)first, (Adaptation)second);
        }
    }
}
