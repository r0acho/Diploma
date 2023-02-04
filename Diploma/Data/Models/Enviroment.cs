namespace Diploma.Data.Models
{
    /// <summary>
    /// Класс для хранения всего, что переедет в переменные окружения (или в хранилище секретов ASP.NET)
    /// Пока храню информацию, которую не страшно показать :)
    /// </summary>
    public static class Enviroment
    {
        /// <summary>
        /// Первая компонента ключа
        /// </summary>
        static byte[] comp1 = Convert.FromHexString("C50E41160302E0F5D6D59F1AA3925C45");

        /// <summary>
        /// Вторая компонента ключа
        /// </summary>
        static byte[] comp2 = Convert.FromHexString("00000000000000000000000000000000");


        /// <summary>
        /// Секретный ключ
        /// </summary>
        static byte[] secretKey;

        /// <summary>
        /// URL ПШ для тестовой среды
        /// </summary>
        static string bankUrl = "https://test.3ds.payment.ru/cgi-bin/cgi_link";

        static Enviroment()
        {
            secretKey = comp1.Zip(comp2, (a, b) => (byte)(a ^ b)).ToArray();
        }
    }
}
