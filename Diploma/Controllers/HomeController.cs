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

        public IActionResult Index()
        {

            //string? host = HttpContext.Request.Host.Value;
            //string? path = HttpContext.Request.Path.Value;
            //string? query = HttpContext.Request.QueryString.Value;
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Pay()
        {
            var pay = new Payment();
            var data = Payment.GetTestModel(); //данные должны приходить извне, но пока источника данных нет
            pay.SetRequestingModel(data);
            return View(data);
        }

        public IActionResult PreAuthorization()
        {
            var pay = new PreAuthorization();
            var data = Payment.GetTestModel(); //данные должны приходить извне, но пока источника данных нет
            pay.SetRequestingModel(data);
            return View(data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}