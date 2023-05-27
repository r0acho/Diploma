using System.Text;
using System.Text.Json;
using Diploma.Application.Interfaces;
using Diploma.Application.Settings;
using Diploma.Domain.Entities;
using Diploma.Domain.Entities.Atol;
using Diploma.Domain.Responses;
using Microsoft.Extensions.Options;

namespace Diploma.Application.Implementations;

public class AtolFiscalizeService : IFiscalizePaymentService
{
    private readonly AtolSettings _atolSettings;

    public AtolFiscalizeService(IOptions<AtolSettings> atolSettings)
    {
        _atolSettings = atolSettings.Value;
    }
    

    public async Task<FiscalizeResponse> FiscalizePayment(SessionStateModel stateModel, RecurringPaymentModel lastPaymentModel, decimal costOfOneKwh)
    {
        TimeSpan timeSinceDate = DateTime.Now.Subtract(_atolSettings.AtolTokenInfo.LastLogin);
        if (timeSinceDate.TotalHours > 23)
        {
            await SetNewToken();
        }
        var receipt = PrepareReceiptData(stateModel, lastPaymentModel, costOfOneKwh);
        return await SendFiscalizeRequest(receipt, stateModel.Id);
    }

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

    private ReceiptData PrepareReceiptData(SessionStateModel stateModel, RecurringPaymentModel lastPaymentModel, decimal costOfOneKwh)
    {
        var receiptData = new ReceiptData();
        receiptData.Receipt.Client.Email = lastPaymentModel.Email!;
        receiptData.Receipt.Company.Email = lastPaymentModel.MerchantEmail!;
        receiptData.Receipt.Company.Sno = _atolSettings.Sno;
        receiptData.Receipt.Company.Inn = _atolSettings.Inn;
        receiptData.Receipt.Company.PaymentAddress = _atolSettings.Address;
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
        receiptData.Receipt.Payments = new List<ReceiptPayment>
        {
            new ReceiptPayment
            {
                Type = _atolSettings.TypeOfPayment,
                Sum = stateModel.SumOfSessionsByBank
            }
        };
        receiptData.Receipt.Total = stateModel.SumOfSessionsByBank;
        receiptData.Receipt.Cashier = _atolSettings.Cashier;
        receiptData.Service.CallbackUrl = _atolSettings.CallbackUrl;
        return receiptData;
    }
    
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