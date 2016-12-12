namespace StudentDriver.Helpers
{
    public static class Weather
    {
        public enum WeatherType
        {
            Sunny,
            Rainy,
            Snowy
        }

        public static string ToString(this WeatherType weatherType)
        {
            switch (weatherType)
            {
                case WeatherType.Sunny:
                    return "sunny";
                case WeatherType.Rainy:
                    return "rainy";
                case WeatherType.Snowy:
                    return "snowy";
            }
            return null;
        }

        public enum TempratureUnit
        {
            F,
            C,
            K
        }

        public static string ToString(this TempratureUnit tempratureUnit)
        {
            switch (tempratureUnit)
            {
                case TempratureUnit.F:
                    return "F";
                case TempratureUnit.C:
                    return "C";
                case TempratureUnit.K:
                    return "K";
            }
            return null;
        }
    }
}
