using System.ComponentModel;
using System.Security.Cryptography;
using Diploma.Domain.Enums;

/// <summary>
/// Класс модели платежа.
/// </summary>
public record PaymentModel
{
    /// <summary>
    /// Сумма платежа.
    /// </summary>
    [DisplayName("AMOUNT")]
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Идентификатор заказа.
    /// </summary>
    [DisplayName("ORDER")]
    public ulong Order { get; set; }

    /// <summary>
    /// Описание платежа.
    /// </summary>
    [DisplayName("DESC")]
    public string? Description { get; set; }

    /// <summary>
    /// Название магазина.
    /// </summary>
    [DisplayName("MERCH_NAME")]
    public string? MerchantName { get; set; }

    /// <summary>
    /// Адрес электронной почты магазина.
    /// </summary>
    [DisplayName("MERCHANT_NOTIFY_EMAIL")]
    public string? MerchantEmail { get; set; }

    /// <summary>
    /// Адрес электронной почты клиента.
    /// </summary>
    [DisplayName("EMAIL")]
    public string? Email { get; set; }

    /// <summary>
    /// Идентификатор магазина.
    /// </summary>
    [DisplayName("MERCHANT")]
    public string? MerchantId { get; set; }

    /// <summary>
    /// Идентификатор терминала магазина.
    /// </summary>
    [DisplayName("TERMINAL")]
    public ulong TerminalId { get; set; }

    /// <summary>
    /// Код валюты.
    /// </summary>
    [DisplayName("CURRENCY")]
    public string? Currency { get; set; } = "RUB";

    /// <summary>
    /// Тип операции.
    /// </summary>
    [DisplayName("TRTYPE")]
    public TrType trType { get; set; }

    /// <summary>
    /// Временная метка запроса в формате yyyyMMddHHmmss.
    /// </summary>
    [DisplayName("TIMESTAMP")]
    public string Timestamp { get; set; } = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

    /// <summary>
    /// Случайное шестнадцатеричное число для идентификации запроса.
    /// </summary>
    [DisplayName("NONCE")]
    public string Nonce { get; set; } = GetRandomHexString();

    /// <summary>
    /// URL, на который будет перенаправлен клиент при завершении платежа.
    /// </summary>
    [DisplayName("BACKREF")]
    public string? ModuleUrl { get; set; }

    /// <summary>
    /// URL, на который будет отправлен POST-запрос с результатами платежа.
    /// </summary>
    [DisplayName("NOTIFY_URL")]
    public string? ModuleNotifyUrl { get; set; }

    /// <summary>
    /// Получает случайную строку шестнадцатеричных символов заданной длины.
    /// </summary>
    /// <param name="length">Длина строки.</param>
    /// <returns>Строка шестнадцатеричных символов.</returns>
    private static string GetRandomHexString(int length = 32)
    {
        using var csprng = RandomNumberGenerator.Create();
        
        // Создаем массив байтов заданной длины и заполняем его случайными значениями
        var bytes = new byte[length];
        csprng.GetNonZeroBytes(bytes);

        // Преобразуем массив байтов в строку шестнадцатеричных символов и возвращаем ее
        return Convert.ToHexString(bytes);
    }
}
