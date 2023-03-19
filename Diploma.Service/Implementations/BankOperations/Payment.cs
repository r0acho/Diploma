using System.Globalization;
using Diploma.Service.Enums;
using Diploma.Service.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Diploma.Domain.Entities;


namespace Diploma.Service.Implementations.BankOperations
{
    /// <summary>
    /// Класс для хранения полей, необходимых для оплаты транзакции в ПСБ
    /// </summary>
    public class Payment : IBankOperationService
    {
        /// <summary>
        /// Тип банковой операции
        /// </summary>
        protected virtual TrType OperationType { get; } = TrType.Pay;

        /// <summary>
        /// Набор полей и значений для проведения операции
        /// </summary>
        protected Dictionary<string, string> _model = new();

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

        /*/// <summary>
        /// Метод, возвращающий тестовую модель
        /// </summary>
        /// <returns>Словарь {string: JsonObject}</returns>
        public static IDictionary<string, object?> GetTestModel()
        {
            return JsonSerializer.Deserialize<ExpandoObject>(MockModels.testJson)!;
        }*/
        
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
                        currentValue = stringElement.Length != 0 ? stringElement.Length + stringElement : "-"; ;
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
        /// <param name="model">Модель, пришедшая извне (от ТАЧ) в формате JSON</param>
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
            _model["BACKREF"] = "http://176.214.127.66:52112"; //захардкоженный IP-адрес модуля, видимого в интернете
            _model["NOTIFY_URL"] = $"{_model["BACKREF"]}/notify";
            _model["P_SIGN"] = CalculatePSign();
            return _model;
        }

        private string GetRandomHexString(int length = 32)
        {
            using var csprng = RandomNumberGenerator.Create();
            var bytes = new byte[length];

            csprng.GetNonZeroBytes(bytes);
            return Convert.ToHexString(bytes);
        }
        
        public IDictionary<string, string> SetRequestingModel(BankOperation model)
        {
            _model["AMOUNT"] = model.Amount.ToString(CultureInfo.CurrentCulture);
            _model["CURRENCY"] = "RUB";
            _model["ORDER"] = model.Order.ToString();
            _model["DESC"] = model.Description;
            _model["TERMINAL"] = "79036777"; // номер терминала, нужно уточнить
            _model["TRTYPE"] = ((int)OperationType).ToString();
            _model["MERCH_NAME"] = model.MerchantName;
            _model["MERCHANT"] = "000599979036777"; //номер тсп, присвоенный банком, нужно уточнить
            _model["EMAIL"] = model.ClientEmail;
            _model["TIMESTAMP"] = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            _model["NONCE"] = GetRandomHexString();
            _model["BACKREF"] = "http://176.214.127.66:52112"; //захардкоженный IP-адрес модуля, видимого в интернете
            _model["NOTIFY_URL"] = $"{_model["BACKREF"]}/notify";
            _model["CARDHOLDER_NOTIFY"] = "EMAIL";
            _model["MERCHANT_NOTIFY"] = "EMAIL";
            _model["MERCHANT_NOTIFY_EMAIL"] = model.MerchantEmail;
            ChangeModelFieldsByInheritMembers();
            _model["P_SIGN"] = CalculatePSign();
            return _model;
        }
        
    }
    
}
