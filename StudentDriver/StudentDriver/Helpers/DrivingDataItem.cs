namespace StudentDriver.Helpers
{
    public class DrivingDataItem
    {
        public double PercentCompletedDouble { get; private set; }
        public string PercentCompletedString { get; private set; }
        public string RatioString { get; private set; }

        public DrivingDataItem(int required, double completed)
        {
            PercentCompletedDouble = completed / required;
            PercentCompletedString = $"{PercentCompletedDouble:P2}";
            RatioString = $"{completed:F1}/{required}";
        }
    }
}
