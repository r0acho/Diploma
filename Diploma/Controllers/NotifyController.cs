using System.Numerics;
using Diploma.Data.Enums;
using Diploma.Data.Interfaces;
using Diploma.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Diploma.Controllers
{
    public class NotifyController : Controller
    {
        //private static bool _isSuccess = false;
        private static string? _textMessageAboutLastOperation;

        private static IRequestingBank GetCurrentOperation(TrType trType)
        {
            return trType switch
            {
                TrType.Pay => new Payment(),
                TrType.Abort => new Abort(),
                TrType.Return => new Return(),
                TrType.PreAuthorization => new PreAuthorization(),
                TrType.EndOfCalculation => new EndOfCalculation(),
                _ => throw new Exception("Нет нужной операции")
            };
        }

        private static IDictionary<string, string> GetReceivedModel(IFormCollection receivedModel)
        {
            var newModel = new Dictionary<string, string>();

            foreach (var pair in receivedModel)
            {
                newModel[pair.Key] = pair.Value.ToString();
            }
            return newModel;
        }

        [HttpPost]
        public void Index()
        {
            var bankPSign = HttpContext.Request.Form["P_SIGN"];
            var trTypeString = HttpContext.Request.Form["TRTYPE"];
            int.TryParse(trTypeString.ToString(), out int result);
            var trType = (TrType)result;
            var operation = GetCurrentOperation(trType);
            string modulePSign = operation.CalculatePSign(GetReceivedModel(Request.Form));

            _textMessageAboutLastOperation = modulePSign == bankPSign ? "Операция успешна" : "Операция НЕ успешна";
        }

        public string CheckStatus()
        {
            return _textMessageAboutLastOperation ?? "Не было операции";
        }
    }
}
