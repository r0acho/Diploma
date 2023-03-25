using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Diploma.Domain.Entities;
using Diploma.Service.Enums;
using Diploma.Service.Interfaces;

namespace Diploma.Service.Implementations.BankOperations;

/// <summary>
///     Класс для подготовки отправляемой модели в банк для оплаты транзакции в ПСБ
/// </summary>
public abstract class BankOperationService : IBankOperationService
{
    /// <summary>
    ///     Набор полей и значений для проведения операции
    /// </summary>
    protected Dictionary<string, string> SendingModel = new();

    protected BankOperation? CurrentBankOperation;
    
    protected IdnMapping Idn = new IdnMapping();

    /// <summary>
    ///     Тип банковой операции
    /// </summary>
    protected abstract TrType OperationType { get; }

    /// <summary>
    ///     Поля, которые нужно отправить для проведения транзакции
    /// </summary>
    protected abstract List<string> RequestKeys { get; init; }

    /// <summary>
    ///     Порядок полей для вычисления параметра P_SIGN
    /// </summary>
    protected abstract List<string> PSignOrder { get; init; }

    public string CalculatePSignOfModel(Dictionary<string, string> model)
    {
        SendingModel = model;
        return CalculatePSign();
    }
    
    private void SetDataFromBankOperationModel()
    {
        SendingModel["AMOUNT"] = CurrentBankOperation!.Amount.ToString(CultureInfo.InvariantCulture);
        SendingModel["ORDER"] = CurrentBankOperation.Order.ToString();
        SendingModel["DESC"] = Idn.GetAscii(CurrentBankOperation.Description!);
        SendingModel["MERCH_NAME"] = Idn.GetAscii(CurrentBankOperation.MerchantName!);
        SendingModel["EMAIL"] = Idn.GetAscii(CurrentBankOperation.ClientEmail!);
        SendingModel["TERMINAL"] = CurrentBankOperation.TerminalId.ToString(); // номер терминала, нужно уточнить
        SendingModel["MERCHANT"] = CurrentBankOperation.MerchantId!; //номер тсп, присвоенный банком, нужно уточнить
    }

    private void SetConstantData()
    {
        SendingModel["CURRENCY"] = "RUB";
        SendingModel["TRTYPE"] = ((int)OperationType).ToString();
        SendingModel["TIMESTAMP"] = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        SendingModel["NONCE"] = GetRandomHexString();
        SendingModel["BACKREF"] = "http://176.214.127.66:52112"; //захардкоженный IP-адрес модуля, видимого в интернете
        SendingModel["NOTIFY_URL"] = $"{SendingModel["BACKREF"]}/notify";
    }
    
    public IDictionary<string, string> GetRequestingModel(BankOperation model)
    {
        CurrentBankOperation = model ?? throw new Exception("Данные от банка null");
        SetDataFromBankOperationModel();
        SetConstantData();
        ChangeModelFieldsByInheritMembers();
        SendingModel["P_SIGN"] = CalculatePSign();
        return SendingModel;
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

        using (var encoder = new HMACSHA256(BankEnvironment.SecretKey))
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
    
    private static string GetRandomHexString(int length = 32)
    {
        using var csprng = RandomNumberGenerator.Create();
        var bytes = new byte[length];

        csprng.GetNonZeroBytes(bytes);
        return Convert.ToHexString(bytes);
    }
}