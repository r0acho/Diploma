using Diploma.Application.Settings;

namespace Diploma.Application.Settings;

/// <summary>
/// Класс настроек для взаимодействия с системой АТОЛ.
/// </summary>
public class AtolSettings
{
    /// <summary>
    /// URL API АТОЛ.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Номер группы ККТ и магазина в системе АТОЛ.
    /// </summary>
    public string GroupCode { get; set; }

    /// <summary>
    /// Логин для авторизации в системе АТОЛ.
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Пароль для авторизации в системе АТОЛ.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// ИНН организации.
    /// </summary>
    public string Inn { get; set; }

    /// <summary>
    /// Адрес места расчетов.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Способ оплаты.
    /// </summary>
    public string PaymentMethod { get; set; }

    /// <summary>
    /// Тип ставки НДС.
    /// </summary>
    public string TypeOfVat { get; set; }

    /// <summary>
    /// Размер налоговой ставки в процентах.
    /// </summary>
    public int TaxRate { get; set; }

    /// <summary>
    /// ФИО кассира.
    /// </summary>
    public string Cashier { get; set; }

    /// <summary>
    /// Тип оплаты (наличные/безналичные).
    /// </summary>
    public int TypeOfPayment { get; set; }

    /// <summary>
    /// Система налогообложения.
    /// </summary>
    public string Sno { get; set; }

    /// <summary>
    /// URL для получения ответа от сервиса обработки чеков.
    /// </summary>
    public string CallbackUrl { get; set; }
    
    /// <summary>
    /// Информация о токене для авторизации в системе АТОЛ.
    /// </summary>
    public AtolTokenInfo AtolTokenInfo { get; set; }
}