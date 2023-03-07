using Diploma.Data.Enums;
using Diploma.Data.Interfaces;
using System.Dynamic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Diploma.Data.Models.BankOperations
{


    /// <summary>
    /// Класс для хранения полей, необходимых для оплаты транзакции в ПСБ
    /// </summary>
    public class Payment : IBankOperations
    {
        /// <summary>
        /// Тип банковой операции
        /// </summary>
        protected virtual TrType OperationType { get; } = TrType.Pay;

        protected Dictionary<string, string> _model = new();
        
        /// <summary>
        /// JSON для тестовой модели
        /// </summary>
        protected static string testJson = $"{{\r\n  \"AMOUNT\": 300.2,\r\n  " +
            $"\"CURRENCY\": \"RUB\",\r\n  \"ORDER\": \"{Random.Shared.Next(10000000, 90000000)}\",\r\n " +
            $" \"DESC\": \"Test Payment\",\r\n  \"TERMINAL\": \"79036777\",\r\n  " +
            $"\"TRTYPE\": 1,\r\n  \"MERCH_NAME\": \"Test Shop\",\r\n  \"MERCHANT\": " +
            $"\"000599979036777\",\r\n  \"EMAIL\": \"cardholder@mail.test\",\r\n  " +
            $"\"TIMESTAMP\": {DateTime.UtcNow.ToString("yyyyMMddHHmmss")},\r\n  \"NONCE\": \"8b495c3669edb02003c2dca666d2182a\"," +
            $"\r\n  \"BACKREF\": \"https://localhost:7269\",\r\n  \"NOTIFY_REF\": " +
            $"\"https://localhost:7269/confirm/\",\r\n  \"CALDHOLDER_NOTIFY\": \"EMAIL\",\r\n  " +
            $"\"MERCHANT_NOTIFY\": \"EMAIL\",\r\n  \"MERCHANT_NOTIFY_EMAIL\": \"merchant@mail.test\", \r\n" +
            /*поля для теста возврата*/
            $"\"org_amount\": 300.2, \r\n \"rrn\": \"911491440337\", \r\n \"int_ref\": \"1ED52C3B234CBAF8\"}}";

        
        /// <summary>
        /// Поля, которые нужно отправить для проведения транзакции
        /// </summary>
        protected virtual List<string> RequestKeys { get; init; } = new List<string> {
            "AMOUNT", "CURRENCY", "ORDER", "DESC", "TERMINAL", "TRTYPE", "MERCH_NAME",
            "MERCHANT","EMAIL", "TIMESTAMP","NONCE","BACKREF","NOTIFY_URL"
        };

        /// <summary>
        /// Порядок полей для вычисления параметра P_SIGN
        /// </summary>
        protected virtual List<string> PSignOrder { get; init; } = new List<string> {
            "AMOUNT", "CURRENCY", "ORDER", "MERCH_NAME", "MERCHANT",
            "TERMINAL", "EMAIL", "TRTYPE", "TIMESTAMP", "NONCE", "BACKREF"
        };

        /// <summary>
        /// Метод, возвращающий тестовую модель
        /// </summary>
        /// <returns>Словарь {string: JsonObject}</returns>
        public static IDictionary<string, object?> GetTestModel()
        {
            return JsonSerializer.Deserialize<ExpandoObject>(testJson)!;
        }

        
        private string ConcatData()
        {
            var concatedKeysBuilder = new StringBuilder();
            foreach (var key in PSignOrder)
            {
                _model.TryGetValue(key, out var value);
                if (value is not null)
                {
                    string currentValue = string.Empty;
                    if (value is string stringElement) 
                    {
                        currentValue = stringElement.Length != 0 ? stringElement.Length.ToString() + stringElement : "-"; ;
                    }
                    concatedKeysBuilder.Append(currentValue);
                }
                else
                {
                    concatedKeysBuilder.Append('-');
                }
            }

            return concatedKeysBuilder.ToString();
        }


        public string CalculatePSignOfModel(Dictionary<string, string> model)
        {
            _model = model;
            return CalculatePSign();
        }
        private string CalculatePSign()
        {
            string concatedKeys = ConcatData();
            byte[] concatedKeysBytes = Encoding.UTF8.GetBytes(concatedKeys);
            byte[] pSignBytes;

            using (var encoder = new HMACSHA256(BankEnvironment.secretKey))
            {
                pSignBytes = encoder.ComputeHash(concatedKeysBytes);
            }
            return Convert.ToHexString(pSignBytes);
        }

        /// <summary>
        /// Метод для подготовки данных по нужным ключам в банк
        /// </summary>
        /// <param name="model">Модель, пришедшая извне (от ТАЧ)</param>
        /// <returns>Готовая к отправке в банк модель</returns>
        private void SetSendingData(IDictionary<string, object?> model)//дальше здесь будет обработка файла ТАЧ
        {
            foreach (var key in RequestKeys)
            {
                model.TryGetValue(key, out object? value);
                if (value is not null && value is JsonElement jsonElement)
                {
                    _model[key] = jsonElement.GetRawText().Replace("\"", string.Empty);
                }
                else
                {
                    _model[key] = string.Empty;
                }
            }

            _model["TRTYPE"] = ((int)OperationType).ToString();
        }

        /// <summary>
        /// Метод для определения необходимых полей в дочерних операциях банка
        /// </summary>
        protected virtual void ChangeModelFieldsByInheritMembers()
        {
            
        }
        
        public IDictionary<string, string> SetRequestingModel(IDictionary<string, object?> model)
        {
            SetSendingData(model);
            ChangeModelFieldsByInheritMembers();
            _model["BACKREF"] = "http://176.214.127.66:52112/"; //захардкоженный IP-адрес модуля, видимого в интернете
            _model["NOTIFY_URL"] = $"{_model["BACKREF"]}/notify/";
            _model["P_SIGN"] = CalculatePSign();
            return _model;
        }
    }
    
}
