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
    }
}
