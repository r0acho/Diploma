using System.Collections.ObjectModel;
using Diploma.Data.Enums;
using Diploma.Data.Interfaces;
using Diploma.Data.Models.BankOperations;
using Diploma.Domain.Entities;

namespace Diploma.Data.Models;

public class SessionHandler : IBankOperationHandler
{
    private const decimal COST_OF_ONE_KWH = 16;
    private IBankOperations? _bankOperation;
    
    public readonly ObservableCollection<BankOperation> Operations = new();
    
    public int OperationId { get; }

    private static IBankOperations GetCurrentOperation(TrType trType)
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
    
    public SessionHandler(int sessionId)
    {
        OperationId = sessionId;
    }

    
    
    private async void HandleSession(int sessionId)
    {

        double a = long.MaxValue;
        BankOperation currentOperation;
        Operations.CollectionChanged += 
            delegate(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)                    
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    currentOperation = Operations.Last();
                    
                }
            };
        
        
    }

    public bool CheckKeys(Dictionary<string, string> bankResponse)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetJsonResult()
    {
        throw new NotImplementedException();
    }
}