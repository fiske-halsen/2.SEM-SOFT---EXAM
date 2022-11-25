namespace EmailService
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }
        // TEST WOW
        public int TemperatureC { get; set; }
        //cheeky little comment!!!
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        //another comment
        public string? Summary { get; set; }
        //wow
        //one more
        //asdasd
        //aasdasd
    }
}