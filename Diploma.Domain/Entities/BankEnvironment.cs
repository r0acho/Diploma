using Microsoft.Extensions.Configuration;

namespace Diploma.Domain.Entities;

public class BankEnvironment
{
    private static IConfiguration _config = new ConfigurationBuilder().AddUserSecrets<BankEnvironment>().Build();
    /// <summary>
    ///     URL ПШ для тестовой среды
    /// </summary>
    public static string BankUrl  => _config["Test:BankUrl"];
    

    /// <summary>
    ///     URL ПШ для генерации платежной ссылки в тестовой среде
    /// </summary>
    public static string GeneratePaymentRef => _config["Test:GenPaymentRefUrl"];

    /// <summary>
    ///     Первая компонента ключа
    /// </summary>
    private static readonly byte[] Comp1 = Convert.FromHexString(_config["Test:FirstComponent"]);

    /// <summary>
    ///     Вторая компонента ключа
    /// </summary>
    private static readonly byte[] Comp2 = Convert.FromHexString(_config["Test:SecondComponent"]);


    /// <summary>
    ///     Секретный ключ
    /// </summary>
    public static readonly byte[] SecretKey;

    static BankEnvironment()
    {
        SecretKey = Comp1.Zip(Comp2, (a, b) => (byte)(a ^ b)).ToArray();
    }
    
}