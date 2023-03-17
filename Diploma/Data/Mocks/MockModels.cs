namespace Diploma.Data.Mocks;

public class MockModels
{
    /// <summary>
    /// JSON для тестовой модели
    /// </summary>
    public static string testJson = $"{{\r\n  \"AMOUNT\": 300.2,\r\n  " +
                                       $"\"CURRENCY\": \"RUB\",\r\n  \"ORDER\": \"911491440337\",\r\n " +
                                       $" \"DESC\": \"Test Payment\",\r\n  \"TERMINAL\": \"79036777\",\r\n  " +
                                       $"\"TRTYPE\": 1,\r\n  \"MERCH_NAME\": \"Test Shop\",\r\n  \"MERCHANT\": " +
                                       $"\"000599979036777\",\r\n  \"EMAIL\": \"cardholder@mail.test\",\r\n  " +
                                       $"\"TIMESTAMP\": {DateTime.UtcNow.ToString("yyyyMMddHHmmss")},\r\n  \"NONCE\": \"8b495c3669edb02003c2dca666d2182a\"," +
                                       $"\r\n  \"BACKREF\": \"https://localhost:7269\",\r\n  \"NOTIFY_REF\": " +
                                       $"\"https://localhost:7269/confirm/\",\r\n  \"CALDHOLDER_NOTIFY\": \"EMAIL\",\r\n  " +
                                       $"\"MERCHANT_NOTIFY\": \"EMAIL\",\r\n  \"MERCHANT_NOTIFY_EMAIL\": \"merchant@mail.test\", \r\n" +
                                       /*поля для теста возврата*/
                                       $"\"org_amount\": 300.2, \r\n \"rrn\": \"911491440337\", \r\n \"int_ref\": \"1ED52C3B234CBAF8\"}}";
}