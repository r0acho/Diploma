using System.Collections.ObjectModel;
using Diploma.Domain.Entities;
using Diploma.Domain.Response;

namespace Diploma.Service.Interfaces;

public interface ISessionHandlerService
{
    internal IBankOperationService BankOperationService { get; }
    internal IAsyncEnumerable<BaseResponse> HandleSessionAsync();
    public ObservableCollection<BankOperation> Operations { get; }

}