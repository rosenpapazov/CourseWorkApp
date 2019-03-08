using System.Collections.Generic;

namespace CourseWorkApp.Data.DTOs
{
    public class OpenWeatherResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Coord Coord { get; set; }
        public Main Main { get; set; }
        public int Dt { get; set; }
        public Wind Wind { get; set; }
        public Sys Sys { get; set; }
        public object Rain { get; set; }
        public object Snow { get; set; }
        public Clouds Clouds { get; set; }
        public List<Weather> Weather { get; set; }
    }

    public class Coord
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }

    public class Wind
    {
        public double Speed { get; set; }
        public int Deg { get; set; }
    }

    public class Sys
    {
        public string Country { get; set; }
    }

    public class Clouds
    {
        public int All { get; set; }
    }

    public class Weather
    {
        public int Id { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
    
    public class Main
    {
        public float Temp { get; set; }
        public float Pressure { get; set; }
        public float Humidity { get; set; }
        public float Temp_min { get; set; }
        public float Temp_max { get; set; }
    }
}
