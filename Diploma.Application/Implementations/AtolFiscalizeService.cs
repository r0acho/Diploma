using System.Text;
using System.Text.Json;
using Diploma.Application.Interfaces;
using Diploma.Application.Settings;
using Diploma.Domain.Entities;
using Diploma.Domain.Entities.Atol;
using Diploma.Domain.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Diploma.Application.Implementations;

/// <summary>
/// Реализация сервиса фискализации платежей с использованием ATOL.
/// </summary>
public class AtolFiscalizeService : IFiscalizePaymentService
{
    private readonly AtolSettings _atolSettings;
    private readonly ILogger<AtolFiscalizeService> _logger;

    public AtolFiscalizeService(IOptions<AtolSettings> atolSettings, ILogger<AtolFiscalizeService> logger)
    {
        _logger = logger;
        _atolSettings = atolSettings.Value;
    }
    
    /// <summary>
    /// Метод фискализации платежа. 
    /// </summary>
    /// <remarks>
    /// Метод производит проверку времени с момента последнего получения токена авторизации. Если это время больше 23 часов,
    /// вызывается метод SetNewToken() для получения нового токена. Затем вызываются методы PrepareReceiptData() и SendFiscalizeRequest(),
    /// которые создают объект ReceiptData и отправляют его на сервер фискализации соответственно. Возвращается объект FiscalizeResponse, 
    /// содержащий информацию о выполнении операции.
    /// </remarks>
    public async Task<FiscalizeResponse> FiscalizePayment(SessionStateModel stateModel, RecurringPaymentModel lastPaymentModel, decimal costOfOneKwh)
    {
        _logger.LogInformation("Началась фискализация чека сессии {StateModelId}", stateModel.Id);
        TimeSpan timeSinceDate = DateTime.Now.Subtract(_atolSettings.AtolTokenInfo.LastLogin);
        if (timeSinceDate.TotalHours > 23)
        {
            await SetNewToken();
        }
        var receipt = PrepareReceiptData(stateModel, lastPaymentModel, costOfOneKwh);
        return await SendFiscalizeRequest(receipt, stateModel.Id);
    }

    /// <summary>
    /// Метод для получения нового токена авторизации в сервисе АТОЛ.
    /// </summary>
    /// <remarks>
    /// Метод отправляет POST-запрос на API сервиса АТОЛ с передачей в теле запроса JSON-объекта,
    /// содержащего логин и пароль пользователя. В ответе на запрос приходит JSON-объект с информацией
    /// о новом токене авторизации, который сохраняется в соответствующем поле объекта настроек.
    /// </remarks>
    private async Task SetNewToken()
    {
        string tokenApiUrl = _atolSettings.Url + "getToken";
        string json = JsonSerializer.Serialize(new { login = _atolSettings.Login, pass = _atolSettings.Password });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        using var client = new HttpClient();
        var response = await client.PostAsync(tokenApiUrl, content);
        response.EnsureSuccessStatusCode();
        _atolSettings.AtolTokenInfo = JsonSerializer.Deserialize<AtolTokenInfo>(await response.Content.ReadAsStringAsync())!;
    }

    /// <summary>
    /// Метод для подготовки данных о чеке на основе переданных моделей.
    /// </summary>
    /// <param name="stateModel"> Модель состояния сессии. </param>
    /// <param name="lastPaymentModel"> Модель последнего платежа. </param>
    /// <param name="costOfOneKwh"> Стоимость одного киловатта-часа. </param>
    /// <returns> Объект класса ReceiptData, содержащий информацию об оформлении чека. </returns>
    private ReceiptData PrepareReceiptData(SessionStateModel stateModel, RecurringPaymentModel lastPaymentModel, decimal costOfOneKwh)
    {
        var receiptData = new ReceiptData();
        receiptData.Receipt.Client.Email = lastPaymentModel.Email!;
        receiptData.Receipt.Company.Email = lastPaymentModel.MerchantEmail!;
        receiptData.Receipt.Company.Sno = _atolSettings.Sno;
        receiptData.Receipt.Company.Inn = _atolSettings.Inn;
        receiptData.Receipt.Company.PaymentAddress = _atolSettings.Address;
        SetReceiptItems(receiptData, stateModel, lastPaymentModel, costOfOneKwh);
        SetReceiptPayments(receiptData, stateModel);
        receiptData.Receipt.Total = stateModel.SumOfSessionsByBank;
        receiptData.Receipt.Cashier = _atolSettings.Cashier;
        receiptData.Service.CallbackUrl = _atolSettings.CallbackUrl;
        return receiptData;
    }

/// <summary>
/// Метод для заполнения списка товаров в объекте ReceiptData на основе переданных моделей.
/// </summary>
/// <param name="receiptData"> Объект ReceiptData, содержащий информацию об оформлении чека. </param>
/// <param name="stateModel"> Модель состояния сессии. </param>
/// <param name="lastPaymentModel"> Модель последнего платежа. </param>
/// <param name="costOfOneKwh"> Стоимость одного киловатта-часа. </param>
private void SetReceiptItems(ReceiptData receiptData, SessionStateModel stateModel, RecurringPaymentModel lastPaymentModel,
    decimal costOfOneKwh)
{
    receiptData.Receipt.Items = new List<ReceiptItem>
    {
        new ReceiptItem
        {
            Name = lastPaymentModel.Description,
            Price = costOfOneKwh,
            Quantity = Math.Round(stateModel.SumOfSessionsByBank / costOfOneKwh, 3),
            Sum = stateModel.SumOfSessionsByBank,
            PaymentMethod = _atolSettings.PaymentMethod,
            Vat = new ReceiptVat
            {
                Type = _atolSettings.TypeOfVat,
                Sum = Math.Round(stateModel.SumOfSessionsByBank / (100 + _atolSettings.TaxRate) * _atolSettings.TaxRate, 2)
            }
        }
    };
}

/// <summary>
/// Метод для заполнения списка платежей в объекте ReceiptData на основе переданной модели состояния сессии.
/// </summary>
/// <param name="receiptData"> Объект ReceiptData, содержащий информацию об оформлении чека. </param>
/// <param name="stateModel"> Модель состояния сессии. </param>
private void SetReceiptPayments(ReceiptData receiptData, SessionStateModel stateModel)
{
    receiptData.Receipt.Payments = new List<ReceiptPayment>
    {
        new ReceiptPayment
        {
            Type = _atolSettings.TypeOfPayment,
            Sum = stateModel.SumOfSessionsByBank
        }
    };
}

/// <summary>
/// Метод для отправки запроса на сервер фискализации с передачей объекта ReceiptData.
/// </summary>
/// <param name="data"> Объект ReceiptData, содержащий информацию об оформлении чека. </param>
/// <param name="sessionId"> Идентификатор сессии. </param>
/// <returns> Объект FiscalizeResponse, содержащий информацию о выполнении операции. </returns>
private async Task<FiscalizeResponse> SendFiscalizeRequest(ReceiptData data, ulong sessionId)
{
    var request = new HttpRequestMessage(HttpMethod.Post, $"{_atolSettings.Url}{_atolSettings.GroupCode}/sell");
    string json = JsonSerializer.Serialize(data);
    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
    request.Headers.Add("Token", _atolSettings.AtolTokenInfo.Token);
    using var client = new HttpClient();
    var response = await client.SendAsync(request);
    string content = await response.Content.ReadAsStringAsync();
    var responseModel = JsonSerializer.Deserialize<FiscalizeResponse>(content)!;
    responseModel.SessionId = sessionId;
    return responseModel;
}
}