using CourseWorkApp.Data.DTOs;
using CourseWorkApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace CourseWorkApp.Controllers
{
    public class HomeController : Controller
    {
        public async Task<OpenWeatherResponseDTO> GetWeatherInformation(string city)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid=2d98b02986293d805f75d3851ee04a42&units=metric");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<OpenWeatherResponseDTO>(stringResult);
                }
                catch (HttpRequestException)
                {
                    throw;
                }
            }
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            ViewData["WeatherInfo"] = await GetWeatherInformation("Svishtov");
            ViewData["Title"] = "Index";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
