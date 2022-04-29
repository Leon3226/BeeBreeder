using BreederComparison = BeeBreeder.Breeding.Comparison.Gene.Comparison;


namespace BeeBreeder.Breeding.Comparison.Gene.Comparators
{
    public class UniversalIntComparator : IGeneValueComparator<int>
    {
        private bool _isLessBetterMode;
        private bool _isMoreBetterMode;

        public bool IsLessBetterMode
        {
            get => _isLessBetterMode;
            set
            {
                ResetAllModes();
                _isLessBetterMode = value;
            }
        }

        public bool IsMoreBetterMode
        {
            get => _isMoreBetterMode;
            set
            {
                ResetAllModes();
                _isMoreBetterMode = value;
            }
        }

        public bool IsIndifferent
        {
            get => !IsLessBetterMode && !IsMoreBetterMode;
            set => ResetAllModes();
        }

        private void ResetAllModes()
        {
            _isLessBetterMode = false;
            _isMoreBetterMode = false;
        }

        public BreederComparison Compare(int first, int second)
        {
            if (IsIndifferent || first == second)
                return BreederComparison.Equal;

            if (IsMoreBetterMode)
            {
                return first > second ? BreederComparison.Better : BreederComparison.Worse;
            }

            if (IsLessBetterMode)
            {
                return first < second ? BreederComparison.Better : BreederComparison.Worse;
            }

            return BreederComparison.Equal;

        }

        public BreederComparison Compare(object first, object second)
        {
            return Compare((int)first, (int)second);
        }
    }
}
