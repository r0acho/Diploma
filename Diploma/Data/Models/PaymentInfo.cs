using Diploma.Data.Interfaces;
using System.Dynamic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Diploma.Data.Models
{
    enum TRTYPE
    {
        PreAuthorization = 12,
        Abort = 22,
        Pay = 1,
        EndOfCalculation = 21,
        Return = 14,
        Reccuring = 171,
        CheckCard = 39
    }

    /// <summary>
    /// Класс для хранения полей, необходимых для оплаты транзакции в ПСБ
    /// </summary>
    public class Payment : IRequestingBank
    {
        /// <summary>
        /// JSON для тестовой модели
        /// </summary>
        static string testJson = $"{{\r\n  \"AMOUNT\": 300.2,\r\n  \"CURRENCY\": \"RUB\",\r\n  \"ORDER\": 123,\r\n  \"DESC\": \"Test Payment\",\r\n  \"TERMINAL\": \"79036777\",\r\n  \"TRTYPE\": 1,\r\n  \"MERCH_NAME\": \"Test Shop\",\r\n  \"MERCHANT\": \"000599979036777\",\r\n  \"EMAIL\": \"cardholder@mail.test\",\r\n  \"TIMESTAMP\": 20230204120000,\r\n  \"NONCE\": \"0xf323f1fad21ab156\",\r\n  \"BACKREF\": \"https://localhost:7269/index\",\r\n  \"NOTIFY_REF\": \"https://localhost:7269/index\",\r\n  \"CALDHOLDER_NOTIFY\": \"EMAIL\",\r\n  \"MERCHANT_NOTIFY\": \"EMAIL\",\r\n  \"MERCHANT_NOTIFY_EMAIL\": \"merchant@mail.test\"\r\n}}";

        /// <summary>
        /// Поля, которые нужно отправить для проведения транзакции
        /// </summary>
        public static readonly List<string> RequestKeys = new List<string> {
        "AMOUNT", "CURRENCY", "ORDER", "DESC", "TERMINAL", "TRTYPE","MERCH_NAME",
        "MERCHANT","EMAIL", "TIMESTAMP","NONCE","BACKREF","NOTIFY_URL","P_SIGN"
        };

        /// <summary>
        /// Порядок полей для вычисления параметра P_SIGN
        /// </summary>
        public static readonly List<string> PSignOrder = new List<string> {
        "AMOUNT", "CURRENCY", "ORDER", "MERCH_NAME", "MERCHANT",
        "TERMINAL", "EMAIL", "TRTYPE", "TIMESTAMP", "NONCE", "BACKREF"
        };

        /// <summary>
        /// Метод, возвращающий тестовую модель
        /// </summary>
        /// <returns>Словарь {string: JsonObject}</returns>
        public static IDictionary<string, object> GetTestModel()
        {
            return JsonSerializer.Deserialize<ExpandoObject>(testJson)!;
        }

        public string CalculatePSign(IDictionary<string, object> model)
        {
            var concatedKeysBuilder = new StringBuilder();
            foreach (var key in PSignOrder)
            {
                model.TryGetValue(key, out object? value);
                if (value is not null)
                {
                    JsonElement el = (JsonElement)value;
                    string currentValue = el.GetRawText().Replace("\"", string.Empty);
                    Console.WriteLine(currentValue);
                    currentValue = currentValue.Length != 0 ? currentValue.Length.ToString() + currentValue : "-";
                    concatedKeysBuilder.Append(currentValue);
                }
                else
                {
                    concatedKeysBuilder.Append('-');
                }
            }

            string concatedKeys = concatedKeysBuilder.ToString();


            byte[] concatedKeysBytes = Encoding.UTF8.GetBytes(concatedKeys);
            byte[] pSignBytes;

            using (var encoder = new HMACSHA256(Enviroment.secretKey))
            {
                pSignBytes = encoder.ComputeHash(concatedKeysBytes);
            }
            return Convert.ToHexString(pSignBytes);
        }

        public IDictionary<string, object> PrepareSendingData(IDictionary<string, object> model)
        {
            string pSign = CalculatePSign(model);
            model["P_SIGN"] = pSign;
            return model;
        }

    }


}
