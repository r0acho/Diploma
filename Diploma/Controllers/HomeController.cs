using Diploma.Data.Models;
using Diploma.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace Diploma.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async void Index()
        {

            string? host = HttpContext.Request.Host.Value;
            string? path = HttpContext.Request.Path.Value;
            string? query = HttpContext.Request.QueryString.Value;

            if(query == "?test")
            {
                var pay = new Payment();
                IDictionary<string, object> data = pay.PrepareSendingData(Payment.GetTestModel());
            }
            var response = new { host = host, path = path, query = query };
            await HttpContext.Response.WriteAsync(
                JsonSerializer.Serialize(response)
            );
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}