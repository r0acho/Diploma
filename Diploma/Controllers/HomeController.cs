using Diploma.Data.Interfaces;
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

        public IDictionary<string, object> PrepareModel (IRequestingBank operation, IDictionary<string, object> model)
        {
            return operation.SetRequestingModel(model);
        }

        public IActionResult Pay()
        {
            return View(PrepareModel(new Payment(), Payment.GetTestModel()));
        }

        public IActionResult PreAuthorization()
        {
            return View(PrepareModel(new PreAuthorization(), Payment.GetTestModel()));
        }

        public IActionResult Return()
        {
            return View(PrepareModel(new Return(), Payment.GetTestModel()));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}