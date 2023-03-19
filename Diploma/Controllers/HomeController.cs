using Diploma.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Dynamic;
using System.Text.Json;
using Diploma.Service.Implementations.BankOperations;
using Diploma.Service.Interfaces;
using Diploma.Data.Mocks;

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

        public IDictionary<string, string> PrepareModel (IBankOperationService operation, IDictionary<string, object?> model)
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

        /*[HttpPost]
        public IActionResult StartPayment()
        {
            HttpContext.Request.
        }*/

        /// <summary>
        /// тестовый метод, удалим
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, object?> GetTestModel()
        {
            return JsonSerializer.Deserialize<ExpandoObject>(MockModels.testJson)!;
        }

        public IActionResult Pay()
        {
            return View(viewName: "SendForm", model: PrepareModel(new Payment(), GetTestModel()));
        }

        public IActionResult PreAuthorization()
        {
            return View(viewName: "SendForm", model: PrepareModel(new PreAuthorization(), GetTestModel()));
        }

        public IActionResult Return()
        {
            return View(viewName: "SendForm", model: PrepareModel(new Return(), GetTestModel()));
        }

        public IActionResult Abort()
        {
            return View(viewName: "SendForm", model: PrepareModel(new Abort(), GetTestModel()));
        }

        public IActionResult EndOfCalculation()
        {
            return View(viewName: "SendForm", model: PrepareModel(new EndOfCalculation(), GetTestModel()));
        }
        
        public IActionResult CheckCard()
        {
            return View(viewName: "SendForm", model: PrepareModel(new CheckCard(), GetTestModel()));
        }
        
        public IActionResult RegRecurring()
        {
            return View(viewName: "SendForm", model: PrepareModel(new ReccuringRegistrarion(), GetTestModel()));
        }
        
        public IActionResult ExeRecurring()
        {
            return View(viewName: "SendForm", model: PrepareModel(new ReccuringExecution(), GetTestModel()));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}