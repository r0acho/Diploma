using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Diploma.Domain.Entities;
using Diploma.Domain.Response;
using Diploma.Service.Implementations.BankOperations;
using Diploma.Service.Interfaces;

namespace Diploma.Service.Implementations;

internal class SessionHandlerService : ISessionHandlerService
{
    private const decimal COST_OF_ONE_KWH = 16;
    private bool _isNewBankOperationCame = false;
    public ObservableCollection<BankOperation> Operations { get; } = new();
    
    private decimal _sumOfSessionsByBank = 0;
    private decimal _sumOfSessionsByTOUCH = 0;

    public SessionHandlerService()
    {
        BankOperationService = new RecurringExecution();
        Operations.CollectionChanged += Operations_CollectionChanged;
    }
    
    public IBankOperationService BankOperationService { get; set; }


    /// <summary>
    ///     Обработчик добавления коллекции
    /// </summary>
    private async void Operations_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            await Task.Run(() =>
            {
                while (_isNewBankOperationCame == true) {} //дождаться, пока не начнем обрабатывать последнюю операцию
                _isNewBankOperationCame = true; //проставить состояние, что пришла новая операция для обработки
            }
            );
        }
    }
    

    private async Task<BaseResponse> GetPaymentRefAsync(BankOperation bankOperation)
    {
        var sendingModel = BankOperationService.GetRequestingModel(bankOperation!);
        var responseMessage = await SendModelToBankAsync(sendingModel);
        string paymentRef = await responseMessage.Content.ReadAsStringAsync();
        if (responseMessage.IsSuccessStatusCode == true)
            return new PaymentReferenceResponse
            {
                StatusCode = responseMessage.StatusCode,
                Description = "Payment ref got successfully",
                PaymentRef = paymentRef
            };
        return new BaseResponse
            {
                StatusCode = responseMessage.StatusCode,
                Description = "Error corrupted while getting payment ref"
            };
    }

    private static async Task<HttpResponseMessage> SendModelToBankAsync(IDictionary<string, string> model)
    {
        using var httpClient = new HttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Post, BankEnvironment.BankUrl);
        request.Content = new FormUrlEncodedContent(model);
        var response = await httpClient.SendAsync(request);
        return response;
    }
    
    private async Task<BaseResponse> ExecuteRecurringPaymentAsync(BankOperation recurringBankOperation)
    {
        var sendingModel = BankOperationService.GetRequestingModel(recurringBankOperation!);
        var responseMessage = await SendModelToBankAsync(sendingModel);
        return new RecurOperationResponse
        {
            StatusCode = responseMessage.StatusCode,
            Description = await responseMessage.Content.ReadAsStringAsync(),
        };
    }

    private async void WaitForNextOperation()
    {
        await Task.Run(() =>
        {
            while (_isNewBankOperationCame == false){}//дождаться, пока не добавят новую операцию
            _isNewBankOperationCame = false; //проставить состояние, что новая операция начинает обрабатываться
        });
    }

    private async IAsyncEnumerable<BaseResponse> ProcessAllPayments()
    {
        //CheckDebt()
        WaitForNextOperation();
        BankOperation bankOperation = Operations.Last();
        yield return await ExecuteRecurringPaymentAsync(bankOperation);
        if (bankOperation.WillSessionContinue == true)
        {
            yield return ProcessAllPayments().GetAsyncEnumerator().Current;
        }
    }
    
    public async IAsyncEnumerable<BaseResponse> HandleSessionAsync()
    {
        await foreach (var recurringOperationResponse in ProcessAllPayments())
        {
            yield return recurringOperationResponse;
        }
        //отправка чеков в АТОЛ
    }

    public bool CheckKeys(Dictionary<string, string> bankResponse)
    {
        throw new NotImplementedException();
    }
    

    private bool CheckDebt()
    {
        throw new NotImplementedException("Нужно проверять долг пользователя в ТАЧ");
    }
}