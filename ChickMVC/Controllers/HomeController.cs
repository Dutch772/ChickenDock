using ChickMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ChickMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IActionResult Index()
        {
            try
            {
                var response = _httpClient.GetStringAsync("https://localhost:8080/api/chicken").Result;
                ViewBag.Chicken = response;
            }
            catch (Exception ex)
            {
                ViewBag.Chicken = "Error: " + ex.Message;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
