using Diploma.Data.Enums;
using Diploma.Data.Interfaces;
using Diploma.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Collections.Specialized;
using System.Reflection;

namespace Diploma.Controllers
{
    public class NotifyController : Controller
    {
        private bool isSuccess = false;

        private IRequestingBank? GetCurrentOperation(TrType trType)
        {
            switch (trType)
            {
                case TrType.Pay:
                    return new Payment();
               case TrType.Abort:
                    return new Abort();
                case TrType.Return:
                    return new Return();
                case TrType.PreAuthorization: 
                    return new PreAuthorization();
                case TrType.EndOfCalculation:
                    return new EndOfCalculation();
                    //остальные кейсы доработать
            }
            return null;
        }

        private IDictionary<string, object> GetReceivedModel(IFormCollection receivedModel)
        {
            var newModel = new Dictionary<string, object>();

            foreach (var key in receivedModel.Keys)
            {
                receivedModel.TryGetValue(key, out StringValues value);
                if (value.ToString() is not null)
                {
                    newModel[key] = value;
                }
                else
                {
                    newModel[key] = string.Empty;
                }
            }
            return newModel;
        }

        [HttpPost]
        public void Index()
        {
            var bankPsign = Request.Form["P_SIGN"];
            var trTypeString = Request.Form["TRTYPE"];
            int.TryParse(trTypeString.ToString(), out int result);
            TrType trType = (TrType)result;

            var operation = GetCurrentOperation(trType);
            string modulePsign = string.Empty;
            if (operation is not null) 
            {
                modulePsign = operation.CalculatePSign(GetReceivedModel(Request.Form));
            }

            isSuccess = modulePsign == bankPsign;
            
        }

        public string CheckStatus()
        {
            return isSuccess ? "Операция прошла успешно, ключи совпали" : "Операция отклонена (ключи не совпали)";
        }
    }
}
