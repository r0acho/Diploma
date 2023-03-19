using System.Diagnostics;
using System.Dynamic;
using System.Text.Json;
using Diploma.Data.Mocks;
using Diploma.Data.Models;
using Diploma.Service.Implementations.BankOperations;
using Diploma.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Controllers;

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

    public IDictionary<string, string> PrepareModel(IBankOperationService operation, IDictionary<string, object?> model)
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
    ///     тестовый метод, удалим
    /// </summary>
    /// <returns></returns>
    private IDictionary<string, object?> GetTestModel()
    {
        return JsonSerializer.Deserialize<ExpandoObject>(MockModels.testJson)!;
    }

    public IActionResult Pay()
    {
        return View("SendForm", PrepareModel(new Payment(), GetTestModel()));
    }

    public IActionResult PreAuthorization()
    {
        return View("SendForm", PrepareModel(new PreAuthorization(), GetTestModel()));
    }

    public IActionResult Return()
    {
        return View("SendForm", PrepareModel(new Return(), GetTestModel()));
    }

    public IActionResult Abort()
    {
        return View("SendForm", PrepareModel(new Abort(), GetTestModel()));
    }

    public IActionResult EndOfCalculation()
    {
        return View("SendForm", PrepareModel(new EndOfCalculation(), GetTestModel()));
    }

    public IActionResult CheckCard()
    {
        return View("SendForm", PrepareModel(new CheckCard(), GetTestModel()));
    }

    public IActionResult RegRecurring()
    {
        return View("SendForm", PrepareModel(new ReccuringRegistrarion(), GetTestModel()));
    }

    public IActionResult ExeRecurring()
    {
        return View("SendForm", PrepareModel(new ReccuringExecution(), GetTestModel()));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}