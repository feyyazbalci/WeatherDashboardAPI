using System.Text.Json.Serialization;

namespace WeatherDashboardAPI.Models.OpenWeatherMap
{
    public class OpenWeatherResponse
    {
        [JsonPropertyName("coord")]
        public Coord? Coord { get; set; }

        [JsonPropertyName("weather")]
        public List<Weather>? Weather { get; set; }

        [JsonPropertyName("main")]
        public Main? Main { get; set; }

        [JsonPropertyName("wind")]
        public Wind? Wind { get; set; }

        [JsonPropertyName("clouds")]
        public Clouds? Clouds { get; set; }

        [JsonPropertyName("rain")]
        public Rain? Rain { get; set; }

        [JsonPropertyName("snow")]
        public Snow? Snow { get; set; }

        [JsonPropertyName("dt")]
        public long Dt { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class Coord
    {
        [JsonPropertyName("lon")]
        public double Lon { get; set; }

        [JsonPropertyName("lat")]
        public double Lat { get; set; }
    }

    public class Weather
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("main")]
        public string Main { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("icon")]
        public string Icon { get; set; } = string.Empty;
    }

    public class Main
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; }

        [JsonPropertyName("temp_min")]
        public double TempMin { get; set; }

        [JsonPropertyName("temp_max")]
        public double TempMax { get; set; }

        [JsonPropertyName("pressure")]
        public int Pressure { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
    }

    public class Wind
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }

        [JsonPropertyName("deg")]
        public int Deg { get; set; }
    }

    public class Clouds
    {
        [JsonPropertyName("all")]
        public int All { get; set; }
    }

    public class Rain
    {
        [JsonPropertyName("1h")]
        public double? OneHour { get; set; }

        [JsonPropertyName("3h")]
        public double? ThreeHours { get; set; }
    }

    public class Snow
    {
        [JsonPropertyName("1h")]
        public double? OneHour { get; set; }

        [JsonPropertyName("3h")]
        public double? ThreeHours { get; set; }
    }
}