namespace Diploma.Application.Settings;

public class BankSettings
{
    /// <summary>
    ///     URL ПШ для тестовой среды
    /// </summary>
    public string BankUrl { get; init; }

    /// <summary>
    ///     Первая компонента ключа
    /// </summary>
    public string FirstComponent { get; init; }

    /// <summary>
    ///     Вторая компонента ключа
    /// </summary>
    public string SecondComponent { get; init; }

    /// <summary>
    ///     API для генерации платежной ссылки
    /// </summary>
    public string GenPaymentRefUrl { get; init; }
    
    /// <summary>
    ///     API для регистрации чека
    /// </summary>
    public string CheckOnlineUrl { get; init; }

    /// <summary>
    ///     Секретный ключ
    /// </summary>
    public byte[] SecretKey => Convert.FromHexString(FirstComponent)
        .Zip(Convert.FromHexString(SecondComponent), (a, b) => (byte)(a ^ b)).ToArray();

}