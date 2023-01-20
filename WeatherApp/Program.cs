using System.Text;
using Newtonsoft.Json;
using WeatherApp;

const string key = "insert your key here";

Console.InputEncoding = Encoding.Unicode;
Console.WriteLine("Это приложение отображает погоду для выбранного города с помощью OpenWeatherMap API");
Console.Write("Введите название города: ");
var city = Console.ReadLine();
var httpClient = new HttpClient();

#region Today forecast

var response = httpClient.GetAsync($"http://api.openweathermap.org/data/2.5/weather?q={city}&APPID={key}&units=metric&lang=ru").Result;
if (response.IsSuccessStatusCode)
{
    var weather = JsonConvert.DeserializeObject<NowWeatherModel>(await response.Content.ReadAsStringAsync());
    var windDirection = weather!.Wind.Deg switch
    {
        >= 0 and < 15 or >= 345 and < 360 => "с",
        >= 15 and < 75 => "св",
        >= 75 and < 105 => "в",
        >= 105 and < 165 => "юв",
        >= 165 and < 195 => "ю",
        >= 195 and < 255 => "юз",
        >= 255 and < 285 => "з",
        >= 285 and < 345 => "сз",
        _ => string.Empty
    };

    Console.WriteLine("----------------------------------------");
    Console.WriteLine("Показывается погода для города: " + weather.Name);
    Console.WriteLine($"Температура: {weather.Main.Temp}, {weather.Weather[0].Description}");
    Console.WriteLine($"Ощущается как: {weather.Main.FeelsLike}");
    Console.WriteLine($"Скорость ветра: {weather.Wind.Speed} м/с, направление: {windDirection}");
    Console.WriteLine($"Влажность: {weather.Main.Humidity}%, давление {weather.Main.Pressure} мм рт. ст.");
}
else
{
    var reader = JsonConvert.DeserializeObject<Error>(await response.Content.ReadAsStringAsync());
    Console.WriteLine($"Ошибка: {reader!.Message}");
}

#endregion

#region 5 day forecast

response = httpClient.GetAsync($"http://api.openweathermap.org/data/2.5/forecast?q={city}&APPID={key}&units=metric&lang=ru").Result;

if (response.IsSuccessStatusCode)
{
    var epoch = new DateTime(1970, 1, 1);
    var weather = JsonConvert.DeserializeObject<ForecastWeatherModel>(await response.Content.ReadAsStringAsync());
    var timezone = weather!.City.Timezone;
    var days = weather.List.GroupBy(x => epoch.AddSeconds(x.Dt + timezone).Date);
    Console.WriteLine("-------------------------------------------------------------------------------------");
    Console.WriteLine($"{"Дата",10} | {"День недели",11} | {"Мин. и макс. температура",24} | {"Описание",25} |");

    foreach (var day in days)
    {
        var min = day.Select(x => x.Main.TempMin).Min();
        var max = day.Select(x => x.Main.TempMax).Max();
        var description = day.ToArray()[0].Weather[0].Description;
        Console.WriteLine($"{day.Key,10:d} | {day.Key.DayOfWeek,11} | {"Мин: " + min,12}{" Макс: " + max,12} | {description,25} |");
    }
}
else
{
    var reader = JsonConvert.DeserializeObject<Error>(await response.Content.ReadAsStringAsync());
    Console.WriteLine($"Ошибка: {reader!.Message}");
}

#endregion