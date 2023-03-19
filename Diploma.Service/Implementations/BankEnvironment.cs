namespace Diploma.Service.Implementations
{
    /// <summary>
    /// Класс для хранения всего, что переедет в переменные окружения (или в хранилище секретов ASP.NET)
    /// Пока храню информацию, которую не страшно показать :)
    /// </summary>
    public static class BankEnvironment
    {
        /// <summary>
        /// Первая компонента ключа
        /// </summary>
        private static byte[] comp1 = Convert.FromHexString("C50E41160302E0F5D6D59F1AA3925C45");

        /// <summary>
        /// Вторая компонента ключа
        /// </summary>
        private static byte[] comp2 = Convert.FromHexString("00000000000000000000000000000000");


        /// <summary>
        /// Секретный ключ
        /// </summary>
        public static byte[] secretKey;

        /// <summary>
        /// URL ПШ для тестовой среды
        /// </summary>
        public static string BankUrl = "https://test.3ds.payment.ru/cgi-bin/cgi_link";
        
        /// <summary>
        /// URL ПШ для генерации платежной ссылки в тестовой среде 
        /// </summary>
        public static string GeneratePaymentRef = "https://test.3ds.payment.ru/cgi-bin/payment_ref/generate_payment_ref";
        
        static BankEnvironment()
        {
            secretKey = comp1.Zip(comp2, (a, b) => (byte)(a ^ b)).ToArray();
        }
    }
}
