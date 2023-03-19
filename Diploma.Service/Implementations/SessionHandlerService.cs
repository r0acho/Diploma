using System.Collections.ObjectModel;
using Diploma.Domain.Entities;
using Diploma.Service.Enums;
using Diploma.Service.Implementations.BankOperations;
using Diploma.Service.Interfaces;

namespace Diploma.Service.Implementations;

public class SessionHandlerService : ISessionHandlerService
{
    private const decimal COST_OF_ONE_KWH = 16;

    public readonly ObservableCollection<BankOperation> Operations = new();
    private decimal _sumOfSessionsByBank = 0;
    private decimal _sumOfSessionsByTOUCH = 0;

    public SessionHandlerService()
    {
        BankOperationService = new GeneratePaymentRef();
        //Operations.CollectionChanged += Operations_CollectionChanged;
    }

    public SessionHandlerService(int sessionId)
    {
        OperationId = sessionId;
        BankOperationService = new Payment();
    }

    public SessionHandlerService(int sessionId, TrType trType)
    {
        OperationId = sessionId;
        BankOperationService = GetCurrentOperation(trType);
    }

    public IBankOperationService BankOperationService { get; }

    public int OperationId { get; }

    /// <summary>
    ///     Обработчик добавления коллекции
    /// </summary>
    /*private void Operations_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            
        }
    }*/
    public async Task<string> GetBankResponse(BankOperation bankOperation)
    {
        var sendingModel = BankOperationService.SetRequestingModel(bankOperation);
        using (var httpClient = new HttpClient())
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, BankEnvironment.GeneratePaymentRef);
            request.Content = new FormUrlEncodedContent(sendingModel);
            using var response = await httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }


    /*private async Task<string> HandleSession(int sessionId)
    {
        await Task.Run(async() =>
            {
                if (Operations.Count != 0 && Operations.Last().WillSessionContinue == false)
                {
                    
                }
            }
        );

    }*/

    public bool CheckKeys(Dictionary<string, string> bankResponse)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetJsonResult()
    {
        throw new NotImplementedException();
    }

    private static IBankOperationService GetCurrentOperation(TrType trType)
    {
        return trType switch
        {
            TrType.Pay => new Payment(),
            TrType.Abort => new Abort(),
            TrType.Return => new Return(),
            TrType.PreAuthorization => new PreAuthorization(),
            TrType.EndOfCalculation => new EndOfCalculation(),
            TrType.Reccuring => new ReccuringExecution(),
            TrType.CheckCard => new CheckCard(),
            _ => throw new Exception("Нет нужной операции")
        };
    }

    private bool CheckDebt()
    {
        throw new NotImplementedException("Нужно проверять долг пользователя в ТАЧ");
    }

    //private delegate Task<string> GetBankResponse();
}