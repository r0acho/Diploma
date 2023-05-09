using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Diploma.Application.Interfaces;
using Diploma.Application.Settings;
using Diploma.Domain.Dto;
using Diploma.Domain.Entities;
using Diploma.Domain.Enums;
using Diploma.Domain.Extensions;

namespace Diploma.Application.Implementations.BankOperations;

/// <summary>
///     Класс для подготовки отправляемой модели в банк для оплаты транзакции в ПСБ
/// </summary>
public abstract class BankOperationService : IBankOperationService
{
    /// <summary>
    ///     Набор полей и значений для проведения операции
    /// </summary>
    protected Dictionary<string, string> SendingModel = new();
    
    protected IdnMapping Idn = new IdnMapping();

    protected PaymentModel _model;
    
    protected readonly BankSettings _bankSettings;

    /// <summary>
    ///     Тип банковой операции
    /// </summary>
    protected abstract TrType OperationType { get; }

    /// <summary>
    ///     Поля, которые нужно отправить для проведения транзакции
    /// </summary>
    protected abstract List<string> RequestKeys { get; }

    /// <summary>
    ///     Порядок полей для вычисления параметра P_SIGN
    /// </summary>
    protected abstract List<string> PSignOrder { get; }
    
    public string CalculatePSignOfModel(Dictionary<string, string> model)
    {
        SendingModel = model;
        return CalculatePSign();
    }

    protected BankOperationService(BankSettings bankSettings)
    {
        _bankSettings = bankSettings;
    }
    
    public BankOperationService(PaymentModel model, BankSettings bankSettings) : this(bankSettings)
    {
        _model = model;
        _model.trType = OperationType;
    }
    
    private string ConcatData()
    {
        var concatedKeysBuilder = new StringBuilder();
        foreach (var key in PSignOrder)
        {
            SendingModel.TryGetValue(key, out var value);
            if (value is not null)
            {
                var currentValue = string.Empty;
                if (value is string stringElement)
                {
                    currentValue = stringElement.Length != 0 ? stringElement.Length + stringElement : "-";
                }

                concatedKeysBuilder.Append(currentValue);
            }
            else
            {
                concatedKeysBuilder.Append('-');
            }
        }

        return concatedKeysBuilder.ToString();
    }

    private string CalculatePSign()
    {
        var concatedKeys = ConcatData();
        var concatedKeysBytes = Encoding.UTF8.GetBytes(concatedKeys);
        byte[] pSignBytes;

        using (var encoder = new HMACSHA256(_bankSettings.SecretKey))
        {
            pSignBytes = encoder.ComputeHash(concatedKeysBytes);
        }

        return Convert.ToHexString(pSignBytes);
    }


    /// <summary>
    ///     Метод для определения необходимых полей в дочерних операциях банка
    /// </summary>
    protected virtual void ChangeModelFieldsByInheritMembers()
    {
    }
    

    public IDictionary<string, string> GetRequestingModel()
    {
        SendingModel = new Dictionary<string, string>(_model.ToKeyValuePairsString(RequestKeys));
        ChangeModelFieldsByInheritMembers();
        SendingModel["P_SIGN"] = CalculatePSign();
        return SendingModel;
    }
}