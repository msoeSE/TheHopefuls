namespace StudentDriver.Helpers
{
    public class DrivingDataItem
    {
        public double PercentCompletedDouble { get; private set; }
        public string PercentCompletedString { get; private set; }
        public string RatioString { get; private set; }
        public double Hours { get; set; }

        public DrivingDataItem(int required, double completed)
        {
            if (required == 0)
            {
                FillZero();
            }
            else
            {
                FillValues(required, completed);
            }

        }

        private void FillValues(int required, double completed)
        {
            Hours = (completed < required) ? completed : required;
            PercentCompletedDouble = (completed<required)? completed / required : 1.0;
            PercentCompletedString = $"{PercentCompletedDouble:P2}";
            RatioString = (completed < required)? $"{completed:F1}/{required}" : $"{required:F1}/{required}";
        }

        private void FillZero()
        {
            PercentCompletedDouble = 0.0;
            PercentCompletedString = $"{PercentCompletedDouble:P2}";
            RatioString = $"{0:F1}/{0}";
        }
    }
}
