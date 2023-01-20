namespace WeatherApp
{
    public class ForecastWeatherModel
    {
        public List[] List { get; set; }
        public City City { get; set; }
    }

    public class City
    {
        public string Name { get; set; }
        public int Timezone { get; set; }
    }

    public class List
    {
        public int Dt { get; set; }
        public Main Main { get; set; }
        public Weather[] Weather { get; set; }
    }
}
