using Diploma.Data.Interfaces;
using Diploma.Data.Models;
using Diploma.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        public IDictionary<string, string> PrepareModel (IRequestingBank operation, IDictionary<string, object?> model)
        {
            return operation.SetRequestingModel(model);
        }

        public FormUrlEncodedContent PrepareModelToForm(IDictionary<string, string> model) 
        { 
            return new FormUrlEncodedContent(model);
        }

        private IActionResult SendForm(IDictionary<string, string> model) 
        { 
            return View(model); 
        }

        public IActionResult Pay()
        {
            return View(viewName: "SendForm", model: PrepareModel(new Payment(), Payment.GetTestModel()));
        }

        public IActionResult PreAuthorization()
        {
            return View(viewName: "SendForm", model: PrepareModel(new PreAuthorization(), Payment.GetTestModel()));
        }

        public IActionResult Return()
        {
            return View(viewName: "SendForm", model: PrepareModel(new Return(), Payment.GetTestModel()));
        }

        public IActionResult Abort()
        {
            return View(viewName: "SendForm", model: PrepareModel(new Abort(), Payment.GetTestModel()));
        }

        public IActionResult EndOfCalculation()
        {
            return View(viewName: "SendForm", model: PrepareModel(new EndOfCalculation(), Payment.GetTestModel()));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}