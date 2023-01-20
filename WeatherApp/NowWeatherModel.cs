using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace WeatherApp
{
    public class NowWeatherModel
    {
        public Weather[] Weather { get; set; }
        public Main Main { get; set; }
        public Wind Wind { get; set; }
        public string Name { get; set; }
    }

    public class Main
    {
        public float Temp { get; set; }

        [JsonProperty("feels_like")]
        public float FeelsLike { get; set; }

        [JsonProperty("temp_min")]
        public float TempMin { get; set; }

        [JsonProperty("temp_max")]
        public float TempMax { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
    }

    public class Wind
    {
        public float Speed { get; set; }
        public int Deg { get; set; }
    }

    public class Weather
    {
        public string Description { get; set; }
    }

    public class Error
    {
        public int Cod { get; set; }
        public string Message { get; set; }

    }
}
