using Microsoft.AspNetCore.Mvc;

namespace Diploma.Controllers
{
    public class NotifyController : Controller
    {
        //private static bool _isSuccess = false;
        private static string? _textMessageAboutLastOperation;

        /*private static IBankOperations GetCurrentOperation(TrType trType)
        {
            return trType switch
            {
                TrType.Pay => new Payment(),
                TrType.Abort => new Abort(),
                TrType.Return => new Return(),
                TrType.PreAuthorization => new PreAuthorization(),
                TrType.EndOfCalculation => new EndOfCalculation(),
                TrType.Reccuring => new ReccuringExecution(),
                TrType.CheckCard => new CheckCard(),
                _ => throw new Exception("Нет нужной операции")
            };
        }*/

        private static Dictionary<string, string> GetReceivedModel(IFormCollection receivedModel)
        {
            var newModel = new Dictionary<string, string>();

            foreach (var pair in receivedModel)
            {
                newModel[pair.Key] = pair.Value.ToString();
            }
            return newModel;
        }

        //сделать обработку ответов банка в отдельном модуле?
        /*[HttpPost]
        public void Index()
        {
            var bankPSign = HttpContext.Request.Form["P_SIGN"];
            var trTypeString = HttpContext.Request.Form["TRTYPE"];
            int.TryParse(trTypeString.ToString(), out int result);
            var trType = (TrType)result;
            var operation = GetCurrentOperation(trType);
            string modulePSign = operation.CalculatePSignOfModel(GetReceivedModel(Request.Form));

            _textMessageAboutLastOperation = modulePSign == bankPSign ? "Операция успешна" : "Операция НЕ успешна";
            HttpContext.Response.WriteAsync(_textMessageAboutLastOperation);
        }*/

        public string CheckStatus()
        {
            return _textMessageAboutLastOperation ?? "Не было операции";
        }
    }
}
